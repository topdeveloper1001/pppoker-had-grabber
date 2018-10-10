using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HandHistories.Objects.GameDescription
{
    [Serializable]
    public struct TableType : IEnumerable<TableTypeDescription>
    {
        #region Statics

        private static readonly Regex ParseRegex = new Regex("[-,;_]");

        public static TableType Parse(string tableType)
        {
            List<string> tableTypeDescriptionStrings = ParseRegex.Split(tableType).ToList();

            return new TableType(
                tableTypeDescriptionStrings
                    .Select(t => (TableTypeDescription)Enum.Parse(typeof(TableTypeDescription), t, true))
                    .Distinct()
                    .ToArray()
                );
        }

        public static TableType FromTableTypeDescriptions(params TableTypeDescription[] tableTypeDescriptions)
        {
            return new TableType(tableTypeDescriptions);
        }

        #endregion

        private readonly TableTypeDescription _tableTypeDescriptions;

        public TableType(params TableTypeDescription[] tableTypeDescriptions)
        {
            _tableTypeDescriptions = TableTypeDescription.Unknown;

            foreach (var item in tableTypeDescriptions)
            {
                _tableTypeDescriptions |= item;
            }
        }

        public TableType(IEnumerable<TableTypeDescription> tableTypeDescriptions)
        {
            _tableTypeDescriptions = TableTypeDescription.Unknown;

            foreach (var item in tableTypeDescriptions)
            {
                _tableTypeDescriptions |= item;
            }
        }

        public TableTypeDescription Descriptions
        {
            get
            {
                return _tableTypeDescriptions;
            }
        }

        public IEnumerator<TableTypeDescription> GetEnumerator()
        {
            return GetTableTypeDescriptions().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetTableTypeDescriptions().GetEnumerator();
        }

        public IEnumerable<TableTypeDescription> GetTableTypeDescriptions()
        {
            var enumToCheck = new TableTypeDescription[]
            {
                TableTypeDescription.Regular, TableTypeDescription.Anonymous, TableTypeDescription.SuperSpeed, TableTypeDescription.Deep,
                TableTypeDescription.Ante, TableTypeDescription.Cap, TableTypeDescription.Speed,
                TableTypeDescription.Jackpot, TableTypeDescription.SevenDeuceGame, TableTypeDescription.FiftyBigBlindsMin,
                TableTypeDescription.Shallow, TableTypeDescription.PushFold, TableTypeDescription.FastFold,
                TableTypeDescription.Strobe, TableTypeDescription.ShortDeck
            };

            var descriptions = _tableTypeDescriptions;
            return enumToCheck.Where(x => descriptions.HasFlag(x));
        }

        public override string ToString()
        {
            return _tableTypeDescriptions.ToString().Replace(", ", "-");
        }    
    }
}