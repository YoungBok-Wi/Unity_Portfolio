#if NBING_THEBACKEND
using BackEnd.Content;
#endif
using System.Collections.Generic;

namespace Library
{
    public partial class RoomTable : TableType
    {
        #region Property
        public string Name { get; private set; }
        public string Desc { get; private set; }
        public string Icon { get; private set; }
        #endregion

        #region Event
        public RoomTable(string _table, string _baseID, string _addID, Dictionary<string, string> _dic)
        {
            Table = _table;
            ID = string.IsNullOrEmpty(_addID) ? _baseID : $"{_baseID}.{_addID}";
            string o = null;
            if (_dic.TryGetValue(_addID + "Name", out o) && !string.IsNullOrEmpty(o))
            {
            	Name = o;
            	_dic.Remove(_addID + "Name");
            }
            m_Data.Add("Name",Name);
            if (_dic.TryGetValue(_addID + "Desc", out o) && !string.IsNullOrEmpty(o))
            {
            	Desc = o;
            	_dic.Remove(_addID + "Desc");
            }
            m_Data.Add("Desc",Desc);
            if (_dic.TryGetValue(_addID + "Icon", out o) && !string.IsNullOrEmpty(o))
            {
            	Icon = o;
            	_dic.Remove(_addID + "Icon");
            }
            m_Data.Add("Icon",Icon);
        }
        #endregion
    }
}
