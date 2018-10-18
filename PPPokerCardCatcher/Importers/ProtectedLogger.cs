//-----------------------------------------------------------------------
// <copyright file="ProtectedLogger.cs" company="Ace Poker Solutions">
// Copyright © 2018 Ace Poker Solutions. All Rights Reserved.
// Unless otherwise noted, all materials contained in this Site are copyrights, 
// trademarks, trade dress and/or other intellectual properties, owned, 
// controlled or licensed by Ace Poker Solutions and may not be used without 
// written consent except as provided in these terms and conditions or in the 
// copyright notice (documents and software) or other proprietary notices 
// provided with the relevant materials.
// </copyright>
//----------------------------------------------------------------------

using PPPokerCardCatcher.Common.Log;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Globalization;

namespace PPPokerCardCatcher.Importers
{
    internal class ProtectedLogger : IProtectedLogger
    {
        private ProtectedLoggerConfiguration configuration;

        private const string PublicKey = "<RSAKeyValue><Modulus>sr2FGWcZwHVaHG3EhV7hVwRiItjDjl9gO3JhT6u90pa5EMJuliEjkRGvXl/Y37n8UPWZq0er2Xl6Zf5HDco9UiX05ZFtZVj6HHQ3rEG81xWzkmp47UEdxP+0RdTM+W60XGqb2+Y6bVJ5SqdYIYJa+HibX1pgdktM3g94enpQmgN+Han9dQXUFU4Nu3d4pSO3TSh+E14CmEkR0Yqmc3PZ5MFcCM8rC0P2+qpw6W6EA3CdbJj2UIj/yf11+nj5U1mGWSVf6XDZj4G3JMjfYMIggMGdGxcCzGg8NQ591agj7IKPKEtgtJAXrVMrzkRstY53kRXo3178ssM89oPcOVh+0w==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        // log life time in number of files
        private const int LogLifetime = 5;
        private StreamWriter streamWriter;
        private string logFile;

        private const int bufferSize = 65536;
        private int messageCounter = 0;

        private bool isInitialized = false;
        private bool isStarted = false;
        private bool isAESKeyAdded = false;

        // random AES key and iv
        private byte[] aesKey;
        private byte[] aesIV;

        /// <summary>
        /// Delete expired logs 
        /// </summary>
        public void CleanLogs()
        {
            if (configuration == null)
            {
                return;
            }

            try
            {
                var logFiles = Directory.GetFiles(configuration.LogDirectory, configuration.LogCleanupTemplate, SearchOption.TopDirectoryOnly);

                foreach (var logFile in logFiles.OrderByDescending(x => x).Skip(LogLifetime))
                {
                    var logFileName = Path.GetFileName(logFile);
                    File.Delete(logFile);
                }
            }
            catch
            {
            }
        }

        public void Initialize(ProtectedLoggerConfiguration configuration)
        {
            if (configuration == null)
            {
                return;
            }

            this.configuration = configuration;

            try
            {
                // Create log directory
                if (!Directory.Exists(configuration.LogDirectory))
                {
                    Directory.CreateDirectory(configuration.LogDirectory);
                }

                // Generate AES key and IV
                using (var aesCryptoProvider = new AesManaged())
                {
                    aesCryptoProvider.GenerateKey();
                    aesCryptoProvider.GenerateIV();

                    aesKey = aesCryptoProvider.Key;
                    aesIV = aesCryptoProvider.IV;
                }

                isInitialized = true;
            }
            catch
            {
                LogProvider.Log.Error("Logger service has not been initialized.");
            }
        }

        /// <summary>
        /// Log message
        /// </summary>
        /// <param name="message">Message to be saved in log</param>
        public void Log(string message)
        {
            if (!isInitialized || !isStarted)
            {
                return;
            }

            try
            {
#if DEBUG               
                streamWriter.WriteLine(message);
                streamWriter.Flush();
                return;
#endif

                // add key to log
                if (!isAESKeyAdded)
                {
                    var aesKeyEncryptedString = CreateAESKeyEncryptedString();
                    streamWriter.WriteLine(aesKeyEncryptedString);
                    streamWriter.Flush();
                    isAESKeyAdded = true;
                }

                var encryptedMessage = CreateAESEncryptedString(message);
                streamWriter.WriteLine(encryptedMessage);

                if (messageCounter >= configuration.MessagesInBuffer)
                {
                    streamWriter.Flush();
                    messageCounter = 0;
                }

                messageCounter++;
            }
            catch
            {
            }
        }

        public void StartLogging()
        {
            if (!isInitialized || isStarted)
            {
                return;
            }

            try
            {
                var logId = DateTime.Now.ToString(configuration.DateFormat, CultureInfo.InvariantCulture);
                logFile = Path.Combine(configuration.LogDirectory, string.Format(configuration.LogTemplate, logId));

                var fileStream = new FileStream(logFile, FileMode.Append, FileAccess.Write, FileShare.Read);
                streamWriter = new StreamWriter(fileStream, Encoding.UTF8, bufferSize);

                isStarted = true;

                messageCounter = 0;
            }
            catch (Exception e)
            {
                LogProvider.Log.Error(this, "Stream logger wasn't started.", e);
            }
        }

        public void StopLogging()
        {
            if (!isStarted || streamWriter == null)
            {
                return;
            }

            try
            {
                messageCounter = 0;
                streamWriter.Flush();
                streamWriter.Close();
            }
            catch (Exception e)
            {
                LogProvider.Log.Error(this, "Stream logger wasn't properly closed.", e);
            }

            isStarted = false;
            isAESKeyAdded = false;
        }

        /// <summary>
        /// Create string with encrypted AES key and iv
        /// </summary>
        /// <returns>Encrypted key and iv as string</returns>
        private string CreateAESKeyEncryptedString()
        {
            var combinedAesKeyAndIV = aesKey.Concat(aesIV).ToArray();
            var encryptedCombinedAesKeyAndIV = EncryptDataWithRSA(combinedAesKeyAndIV);

            var aesKeyData = new byte[encryptedCombinedAesKeyAndIV.Length + 1];

            // copy encrypted aes data to prepared array
            encryptedCombinedAesKeyAndIV.CopyTo(aesKeyData, 1);

            // set flag [key or data] to key
            aesKeyData[0] = (byte)DataFlag.Key;

            var aesKeyEncryptedString = Convert.ToBase64String(aesKeyData);

            return aesKeyEncryptedString;
        }

        /// <summary>
        /// Create encrypted with AES string
        /// </summary>
        /// <param name="message">String to be encrypted</param>
        /// <returns>Encrypted with AES string</returns>
        private string CreateAESEncryptedString(string message)
        {
            var data = Encoding.UTF8.GetBytes(message);
            var encryptedData = EncryptDataWithAES(data);

            var date = DateTime.Now.ToString(configuration.DateTimeFormat, CultureInfo.InvariantCulture);
            var dateBytes = Encoding.UTF8.GetBytes(date);

            var messageData = new byte[dateBytes.Length + encryptedData.Length + 1];

            dateBytes.CopyTo(messageData, 1);
            encryptedData.CopyTo(messageData, 1 + dateBytes.Length);

            messageData[0] = (byte)DataFlag.Data;

            var aesEncryptedString = Convert.ToBase64String(messageData);

            return aesEncryptedString;
        }

        /// <summary>
        /// Encrypt data with RSA public key
        /// </summary>
        /// <param name="data">Data to be encrypt</param>
        /// <returns>Encrypted data</returns>
        private static byte[] EncryptDataWithRSA(byte[] data)
        {
            using (var cipher = new RSACryptoServiceProvider(2048))
            {
                cipher.FromXmlString(PublicKey);

                var encryptedData = cipher.Encrypt(data, false);

                return encryptedData;
            }
        }

        /// <summary>
        /// Encrypt data with aes key
        /// </summary>
        /// <param name="data">Data to be encrypt</param>
        /// <returns>Encrypted data</returns>
        private byte[] EncryptDataWithAES(byte[] data)
        {
            byte[] encryptedData;

            using (var aesCryptoProvider = new AesManaged())
            {
                aesCryptoProvider.Key = aesKey;
                aesCryptoProvider.IV = aesIV;

                var encryptor = aesCryptoProvider.CreateEncryptor();

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new BinaryWriter(csEncrypt))
                        {
                            swEncrypt.Write(data);
                        }

                        encryptedData = msEncrypt.ToArray();
                        return encryptedData;
                    }
                }
            }
        }

        enum DataFlag : byte
        {
            Data = 0,
            Key = 1
        }
    }
}