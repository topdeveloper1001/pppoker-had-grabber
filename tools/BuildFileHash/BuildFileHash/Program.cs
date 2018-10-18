using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BuildFileHash
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: BuildFileHash <filepath>");
                return;
            }

            var file = args[0];

            if (!File.Exists(file))
            {
                Console.WriteLine("Error: File doesn't exist");
                return;
            }
            try
            {
                var fileHash = BuildFileHash(file);
                Console.WriteLine(fileHash);
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error: {0}", ex.Message));
            }
        }

        static string BuildFileHash(string file)
        {
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                var sha1CryptoProvider = new SHA1CryptoServiceProvider();

                var hashBytes = sha1CryptoProvider.ComputeHash(fs);

                var sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                var fileHash = sb.ToString();

                return fileHash;
            }
        }
    }
}