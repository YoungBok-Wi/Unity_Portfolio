#if NBING_THEBACKEND
using BackEnd.Content;
#endif
using System.Collections.Generic;

namespace Library
{
    public partial class CharacterTable : TableType
    {
        #region Property
        public string Name { get; private set; }
        public string Desc { get; private set; }
        public string Icon { get; private set; }
        public string WeaponType { get; private set; }
        public int Hp { get; private set; }
        public int Attack1 { get; private set; }
        public int Attack2 { get; private set; }
        public int Attack3 { get; private set; }
        public float AttackInterval { get; private set; }
        public float InputBuffer { get; private set; }
        public float MoveSpeed { get; private set; }
        public float RangeWidth { get; private set; }
        public float RangeHeight { get; private set; }
        public float ProjectileSpeed { get; private set; }
        public int HitMax { get; private set; }
        public int Pierce { get; private set; }
        public float KnockbackDist { get; private set; }
        public float KnockbackTime { get; private set; }
        public float KnockbackDistFinish { get; private set; }
        public float KnockbackTimeFinish { get; private set; }
        public int UnlockRoom { get; private set; }
        #endregion

        #region Event
        public CharacterTable(string _table, string _baseID, string _addID, Dictionary<string, string> _dic)
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
            if (_dic.TryGetValue(_addID + "WeaponType", out o) && !string.IsNullOrEmpty(o))
            {
            	WeaponType = o;
            	_dic.Remove(_addID + "WeaponType");
            }
            m_Data.Add("WeaponType",WeaponType);
            if (_dic.TryGetValue(_addID + "Hp", out o) && !string.IsNullOrEmpty(o))
            {
            	Hp = int.Parse(o);
            	_dic.Remove(_addID + "Hp");
            }
            m_Data.Add("Hp",Hp);
            if (_dic.TryGetValue(_addID + "Attack1", out o) && !string.IsNullOrEmpty(o))
            {
            	Attack1 = int.Parse(o);
            	_dic.Remove(_addID + "Attack1");
            }
            m_Data.Add("Attack1",Attack1);
            if (_dic.TryGetValue(_addID + "Attack2", out o) && !string.IsNullOrEmpty(o))
            {
            	Attack2 = int.Parse(o);
            	_dic.Remove(_addID + "Attack2");
            }
            m_Data.Add("Attack2",Attack2);
            if (_dic.TryGetValue(_addID + "Attack3", out o) && !string.IsNullOrEmpty(o))
            {
            	Attack3 = int.Parse(o);
            	_dic.Remove(_addID + "Attack3");
            }
            m_Data.Add("Attack3",Attack3);
            if (_dic.TryGetValue(_addID + "AttackInterval", out o) && !string.IsNullOrEmpty(o))
            {
            	AttackInterval = float.Parse(o);
            	_dic.Remove(_addID + "AttackInterval");
            }
            m_Data.Add("AttackInterval",AttackInterval);
            if (_dic.TryGetValue(_addID + "InputBuffer", out o) && !string.IsNullOrEmpty(o))
            {
            	InputBuffer = float.Parse(o);
            	_dic.Remove(_addID + "InputBuffer");
            }
            m_Data.Add("InputBuffer",InputBuffer);
            if (_dic.TryGetValue(_addID + "MoveSpeed", out o) && !string.IsNullOrEmpty(o))
            {
            	MoveSpeed = float.Parse(o);
            	_dic.Remove(_addID + "MoveSpeed");
            }
            m_Data.Add("MoveSpeed",MoveSpeed);
            if (_dic.TryGetValue(_addID + "RangeWidth", out o) && !string.IsNullOrEmpty(o))
            {
            	RangeWidth = float.Parse(o);
            	_dic.Remove(_addID + "RangeWidth");
            }
            m_Data.Add("RangeWidth",RangeWidth);
            if (_dic.TryGetValue(_addID + "RangeHeight", out o) && !string.IsNullOrEmpty(o))
            {
            	RangeHeight = float.Parse(o);
            	_dic.Remove(_addID + "RangeHeight");
            }
            m_Data.Add("RangeHeight",RangeHeight);
            if (_dic.TryGetValue(_addID + "ProjectileSpeed", out o) && !string.IsNullOrEmpty(o))
            {
            	ProjectileSpeed = float.Parse(o);
            	_dic.Remove(_addID + "ProjectileSpeed");
            }
            m_Data.Add("ProjectileSpeed",ProjectileSpeed);
            if (_dic.TryGetValue(_addID + "HitMax", out o) && !string.IsNullOrEmpty(o))
            {
            	HitMax = int.Parse(o);
            	_dic.Remove(_addID + "HitMax");
            }
            m_Data.Add("HitMax",HitMax);
            if (_dic.TryGetValue(_addID + "Pierce", out o) && !string.IsNullOrEmpty(o))
            {
            	Pierce = int.Parse(o);
            	_dic.Remove(_addID + "Pierce");
            }
            m_Data.Add("Pierce",Pierce);
            if (_dic.TryGetValue(_addID + "KnockbackDist", out o) && !string.IsNullOrEmpty(o))
            {
            	KnockbackDist = float.Parse(o);
            	_dic.Remove(_addID + "KnockbackDist");
            }
            m_Data.Add("KnockbackDist",KnockbackDist);
            if (_dic.TryGetValue(_addID + "KnockbackTime", out o) && !string.IsNullOrEmpty(o))
            {
            	KnockbackTime = float.Parse(o);
            	_dic.Remove(_addID + "KnockbackTime");
            }
            m_Data.Add("KnockbackTime",KnockbackTime);
            if (_dic.TryGetValue(_addID + "KnockbackDistFinish", out o) && !string.IsNullOrEmpty(o))
            {
            	KnockbackDistFinish = float.Parse(o);
            	_dic.Remove(_addID + "KnockbackDistFinish");
            }
            m_Data.Add("KnockbackDistFinish",KnockbackDistFinish);
            if (_dic.TryGetValue(_addID + "KnockbackTimeFinish", out o) && !string.IsNullOrEmpty(o))
            {
            	KnockbackTimeFinish = float.Parse(o);
            	_dic.Remove(_addID + "KnockbackTimeFinish");
            }
            m_Data.Add("KnockbackTimeFinish",KnockbackTimeFinish);
            if (_dic.TryGetValue(_addID + "UnlockRoom", out o) && !string.IsNullOrEmpty(o))
            {
            	UnlockRoom = int.Parse(o);
            	_dic.Remove(_addID + "UnlockRoom");
            }
            m_Data.Add("UnlockRoom",UnlockRoom);
        }
        #endregion
    }
}
