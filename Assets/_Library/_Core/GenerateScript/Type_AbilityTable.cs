#if NBING_THEBACKEND
using BackEnd.Content;
#endif
using System.Collections.Generic;

namespace Library
{
    public partial class AbilityTable : TableType
    {
        #region Property
        public string Name { get; private set; }
        public string Desc { get; private set; }
        public string Icon { get; private set; }
        public string Category { get; private set; }
        public string StackMode { get; private set; }
        public float Value { get; private set; }
        public float ValueSub { get; private set; }
        public int MaxStack { get; private set; }
        #endregion

        #region Event
        public AbilityTable(string _table, string _baseID, string _addID, Dictionary<string, string> _dic)
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
            if (_dic.TryGetValue(_addID + "Category", out o) && !string.IsNullOrEmpty(o))
            {
            	Category = o;
            	_dic.Remove(_addID + "Category");
            }
            m_Data.Add("Category",Category);
            if (_dic.TryGetValue(_addID + "StackMode", out o) && !string.IsNullOrEmpty(o))
            {
            	StackMode = o;
            	_dic.Remove(_addID + "StackMode");
            }
            m_Data.Add("StackMode",StackMode);
            if (_dic.TryGetValue(_addID + "Value", out o) && !string.IsNullOrEmpty(o))
            {
            	Value = float.Parse(o);
            	_dic.Remove(_addID + "Value");
            }
            m_Data.Add("Value",Value);
            if (_dic.TryGetValue(_addID + "ValueSub", out o) && !string.IsNullOrEmpty(o))
            {
            	ValueSub = float.Parse(o);
            	_dic.Remove(_addID + "ValueSub");
            }
            m_Data.Add("ValueSub",ValueSub);
            if (_dic.TryGetValue(_addID + "MaxStack", out o) && !string.IsNullOrEmpty(o))
            {
            	MaxStack = int.Parse(o);
            	_dic.Remove(_addID + "MaxStack");
            }
            m_Data.Add("MaxStack",MaxStack);
        }
        #endregion
    }
}
