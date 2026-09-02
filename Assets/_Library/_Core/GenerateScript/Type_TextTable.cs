#if NBING_THEBACKEND
using BackEnd.Content;
#endif
using System.Collections.Generic;

namespace Library
{
    public partial class TextTable : TableType
    {
        #region Property
        public TextData Name { get; private set; }
        #endregion

        #region Event
        public TextTable(string _table, string _baseID, string _addID, Dictionary<string, string> _dic)
        {
            Table = _table;
            ID = string.IsNullOrEmpty(_addID) ? _baseID : $"{_baseID}.{_addID}";
            string o = null;
            Name = new(_table, ID, _addID + "Name", _dic);
            m_Data.Add("Name",Name);
        }
        #endregion
    }
}
