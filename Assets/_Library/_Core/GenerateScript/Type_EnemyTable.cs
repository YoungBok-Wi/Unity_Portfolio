#if NBING_THEBACKEND
using BackEnd.Content;
#endif
using System.Collections.Generic;

namespace Library
{
    public partial class EnemyTable : TableType
    {
        #region Property
        public string Name { get; private set; }
        public string Desc { get; private set; }
        public string Icon { get; private set; }
        public string Group { get; private set; }
        public int Hp { get; private set; }
        public int Attack { get; private set; }
        public float AttackInterval { get; private set; }
        public float MoveSpeed { get; private set; }
        public float StopDistance { get; private set; }
        public float Range { get; private set; }
        public float ProjectileSpeed { get; private set; }
        public float HitboxWidth { get; private set; }
        public float Spacing { get; private set; }
        public float KnockbackRate { get; private set; }
        public int CrumbDrop { get; private set; }
        #endregion

        #region Event
        public EnemyTable(string _table, string _baseID, string _addID, Dictionary<string, string> _dic)
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
            if (_dic.TryGetValue(_addID + "Group", out o) && !string.IsNullOrEmpty(o))
            {
            	Group = o;
            	_dic.Remove(_addID + "Group");
            }
            m_Data.Add("Group",Group);
            if (_dic.TryGetValue(_addID + "Hp", out o) && !string.IsNullOrEmpty(o))
            {
            	Hp = int.Parse(o);
            	_dic.Remove(_addID + "Hp");
            }
            m_Data.Add("Hp",Hp);
            if (_dic.TryGetValue(_addID + "Attack", out o) && !string.IsNullOrEmpty(o))
            {
            	Attack = int.Parse(o);
            	_dic.Remove(_addID + "Attack");
            }
            m_Data.Add("Attack",Attack);
            if (_dic.TryGetValue(_addID + "AttackInterval", out o) && !string.IsNullOrEmpty(o))
            {
            	AttackInterval = float.Parse(o);
            	_dic.Remove(_addID + "AttackInterval");
            }
            m_Data.Add("AttackInterval",AttackInterval);
            if (_dic.TryGetValue(_addID + "MoveSpeed", out o) && !string.IsNullOrEmpty(o))
            {
            	MoveSpeed = float.Parse(o);
            	_dic.Remove(_addID + "MoveSpeed");
            }
            m_Data.Add("MoveSpeed",MoveSpeed);
            if (_dic.TryGetValue(_addID + "StopDistance", out o) && !string.IsNullOrEmpty(o))
            {
            	StopDistance = float.Parse(o);
            	_dic.Remove(_addID + "StopDistance");
            }
            m_Data.Add("StopDistance",StopDistance);
            if (_dic.TryGetValue(_addID + "Range", out o) && !string.IsNullOrEmpty(o))
            {
            	Range = float.Parse(o);
            	_dic.Remove(_addID + "Range");
            }
            m_Data.Add("Range",Range);
            if (_dic.TryGetValue(_addID + "ProjectileSpeed", out o) && !string.IsNullOrEmpty(o))
            {
            	ProjectileSpeed = float.Parse(o);
            	_dic.Remove(_addID + "ProjectileSpeed");
            }
            m_Data.Add("ProjectileSpeed",ProjectileSpeed);
            if (_dic.TryGetValue(_addID + "HitboxWidth", out o) && !string.IsNullOrEmpty(o))
            {
            	HitboxWidth = float.Parse(o);
            	_dic.Remove(_addID + "HitboxWidth");
            }
            m_Data.Add("HitboxWidth",HitboxWidth);
            if (_dic.TryGetValue(_addID + "Spacing", out o) && !string.IsNullOrEmpty(o))
            {
            	Spacing = float.Parse(o);
            	_dic.Remove(_addID + "Spacing");
            }
            m_Data.Add("Spacing",Spacing);
            if (_dic.TryGetValue(_addID + "KnockbackRate", out o) && !string.IsNullOrEmpty(o))
            {
            	KnockbackRate = float.Parse(o);
            	_dic.Remove(_addID + "KnockbackRate");
            }
            m_Data.Add("KnockbackRate",KnockbackRate);
            if (_dic.TryGetValue(_addID + "CrumbDrop", out o) && !string.IsNullOrEmpty(o))
            {
            	CrumbDrop = int.Parse(o);
            	_dic.Remove(_addID + "CrumbDrop");
            }
            m_Data.Add("CrumbDrop",CrumbDrop);
        }
        #endregion
    }
}
