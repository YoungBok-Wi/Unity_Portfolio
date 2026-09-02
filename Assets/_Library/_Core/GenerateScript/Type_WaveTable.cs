#if NBING_THEBACKEND
using BackEnd.Content;
#endif
using System.Collections.Generic;

namespace Library
{
    public partial class WaveTable : TableType
    {
        #region Property
        public int RoomMin { get; private set; }
        public int RoomMax { get; private set; }
        public int WaveIndex { get; private set; }
        public string Enemy1Id { get; private set; }
        public int Enemy1Count { get; private set; }
        public string Enemy2Id { get; private set; }
        public int Enemy2Count { get; private set; }
        public string Enemy3Id { get; private set; }
        public int Enemy3Count { get; private set; }
        #endregion

        #region Event
        public WaveTable(string _table, string _baseID, string _addID, Dictionary<string, string> _dic)
        {
            Table = _table;
            ID = string.IsNullOrEmpty(_addID) ? _baseID : $"{_baseID}.{_addID}";
            string o = null;
            if (_dic.TryGetValue(_addID + "RoomMin", out o) && !string.IsNullOrEmpty(o))
            {
            	RoomMin = int.Parse(o);
            	_dic.Remove(_addID + "RoomMin");
            }
            m_Data.Add("RoomMin",RoomMin);
            if (_dic.TryGetValue(_addID + "RoomMax", out o) && !string.IsNullOrEmpty(o))
            {
            	RoomMax = int.Parse(o);
            	_dic.Remove(_addID + "RoomMax");
            }
            m_Data.Add("RoomMax",RoomMax);
            if (_dic.TryGetValue(_addID + "WaveIndex", out o) && !string.IsNullOrEmpty(o))
            {
            	WaveIndex = int.Parse(o);
            	_dic.Remove(_addID + "WaveIndex");
            }
            m_Data.Add("WaveIndex",WaveIndex);
            if (_dic.TryGetValue(_addID + "Enemy1Id", out o) && !string.IsNullOrEmpty(o))
            {
            	Enemy1Id = o;
            	_dic.Remove(_addID + "Enemy1Id");
            }
            m_Data.Add("Enemy1Id",Enemy1Id);
            if (_dic.TryGetValue(_addID + "Enemy1Count", out o) && !string.IsNullOrEmpty(o))
            {
            	Enemy1Count = int.Parse(o);
            	_dic.Remove(_addID + "Enemy1Count");
            }
            m_Data.Add("Enemy1Count",Enemy1Count);
            if (_dic.TryGetValue(_addID + "Enemy2Id", out o) && !string.IsNullOrEmpty(o))
            {
            	Enemy2Id = o;
            	_dic.Remove(_addID + "Enemy2Id");
            }
            m_Data.Add("Enemy2Id",Enemy2Id);
            if (_dic.TryGetValue(_addID + "Enemy2Count", out o) && !string.IsNullOrEmpty(o))
            {
            	Enemy2Count = int.Parse(o);
            	_dic.Remove(_addID + "Enemy2Count");
            }
            m_Data.Add("Enemy2Count",Enemy2Count);
            if (_dic.TryGetValue(_addID + "Enemy3Id", out o) && !string.IsNullOrEmpty(o))
            {
            	Enemy3Id = o;
            	_dic.Remove(_addID + "Enemy3Id");
            }
            m_Data.Add("Enemy3Id",Enemy3Id);
            if (_dic.TryGetValue(_addID + "Enemy3Count", out o) && !string.IsNullOrEmpty(o))
            {
            	Enemy3Count = int.Parse(o);
            	_dic.Remove(_addID + "Enemy3Count");
            }
            m_Data.Add("Enemy3Count",Enemy3Count);
        }
        #endregion
    }
}
