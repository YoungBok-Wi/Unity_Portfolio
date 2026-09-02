#if NBING_THEBACKEND
using BackEnd.Content;
#endif
using System.Collections.Generic;

namespace Library
{
    public partial class BossTable : TableType
    {
        #region Property
        public string Name { get; private set; }
        public string Desc { get; private set; }
        public string Icon { get; private set; }
        public string AttackType { get; private set; }
        public int Hp { get; private set; }
        public float MoveSpeed { get; private set; }
        public float EnrageMoveSpeed { get; private set; }
        public float HoldDistance { get; private set; }
        public float EnrageHpRatio { get; private set; }
        public int CrumbDrop { get; private set; }
        public string Skill1Id { get; private set; }
        public float Skill1Telegraph { get; private set; }
        public int Skill1Damage { get; private set; }
        public float Skill1Range { get; private set; }
        public float Skill1Interval { get; private set; }
        public float Skill1EnrageInterval { get; private set; }
        public float Skill1ProjectileSpeed { get; private set; }
        public int Skill1Count { get; private set; }
        public float Skill1CountInterval { get; private set; }
        public string Skill2Id { get; private set; }
        public float Skill2Telegraph { get; private set; }
        public int Skill2Damage { get; private set; }
        public float Skill2TriggerDistance { get; private set; }
        public float Skill2Speed { get; private set; }
        public float Skill2Interval { get; private set; }
        public float Skill2AreaWidth { get; private set; }
        public int Skill2AreaCount { get; private set; }
        public int Skill2EnrageAreaCount { get; private set; }
        #endregion

        #region Event
        public BossTable(string _table, string _baseID, string _addID, Dictionary<string, string> _dic)
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
            if (_dic.TryGetValue(_addID + "AttackType", out o) && !string.IsNullOrEmpty(o))
            {
            	AttackType = o;
            	_dic.Remove(_addID + "AttackType");
            }
            m_Data.Add("AttackType",AttackType);
            if (_dic.TryGetValue(_addID + "Hp", out o) && !string.IsNullOrEmpty(o))
            {
            	Hp = int.Parse(o);
            	_dic.Remove(_addID + "Hp");
            }
            m_Data.Add("Hp",Hp);
            if (_dic.TryGetValue(_addID + "MoveSpeed", out o) && !string.IsNullOrEmpty(o))
            {
            	MoveSpeed = float.Parse(o);
            	_dic.Remove(_addID + "MoveSpeed");
            }
            m_Data.Add("MoveSpeed",MoveSpeed);
            if (_dic.TryGetValue(_addID + "EnrageMoveSpeed", out o) && !string.IsNullOrEmpty(o))
            {
            	EnrageMoveSpeed = float.Parse(o);
            	_dic.Remove(_addID + "EnrageMoveSpeed");
            }
            m_Data.Add("EnrageMoveSpeed",EnrageMoveSpeed);
            if (_dic.TryGetValue(_addID + "HoldDistance", out o) && !string.IsNullOrEmpty(o))
            {
            	HoldDistance = float.Parse(o);
            	_dic.Remove(_addID + "HoldDistance");
            }
            m_Data.Add("HoldDistance",HoldDistance);
            if (_dic.TryGetValue(_addID + "EnrageHpRatio", out o) && !string.IsNullOrEmpty(o))
            {
            	EnrageHpRatio = float.Parse(o);
            	_dic.Remove(_addID + "EnrageHpRatio");
            }
            m_Data.Add("EnrageHpRatio",EnrageHpRatio);
            if (_dic.TryGetValue(_addID + "CrumbDrop", out o) && !string.IsNullOrEmpty(o))
            {
            	CrumbDrop = int.Parse(o);
            	_dic.Remove(_addID + "CrumbDrop");
            }
            m_Data.Add("CrumbDrop",CrumbDrop);
            if (_dic.TryGetValue(_addID + "Skill1Id", out o) && !string.IsNullOrEmpty(o))
            {
            	Skill1Id = o;
            	_dic.Remove(_addID + "Skill1Id");
            }
            m_Data.Add("Skill1Id",Skill1Id);
            if (_dic.TryGetValue(_addID + "Skill1Telegraph", out o) && !string.IsNullOrEmpty(o))
            {
            	Skill1Telegraph = float.Parse(o);
            	_dic.Remove(_addID + "Skill1Telegraph");
            }
            m_Data.Add("Skill1Telegraph",Skill1Telegraph);
            if (_dic.TryGetValue(_addID + "Skill1Damage", out o) && !string.IsNullOrEmpty(o))
            {
            	Skill1Damage = int.Parse(o);
            	_dic.Remove(_addID + "Skill1Damage");
            }
            m_Data.Add("Skill1Damage",Skill1Damage);
            if (_dic.TryGetValue(_addID + "Skill1Range", out o) && !string.IsNullOrEmpty(o))
            {
            	Skill1Range = float.Parse(o);
            	_dic.Remove(_addID + "Skill1Range");
            }
            m_Data.Add("Skill1Range",Skill1Range);
            if (_dic.TryGetValue(_addID + "Skill1Interval", out o) && !string.IsNullOrEmpty(o))
            {
            	Skill1Interval = float.Parse(o);
            	_dic.Remove(_addID + "Skill1Interval");
            }
            m_Data.Add("Skill1Interval",Skill1Interval);
            if (_dic.TryGetValue(_addID + "Skill1EnrageInterval", out o) && !string.IsNullOrEmpty(o))
            {
            	Skill1EnrageInterval = float.Parse(o);
            	_dic.Remove(_addID + "Skill1EnrageInterval");
            }
            m_Data.Add("Skill1EnrageInterval",Skill1EnrageInterval);
            if (_dic.TryGetValue(_addID + "Skill1ProjectileSpeed", out o) && !string.IsNullOrEmpty(o))
            {
            	Skill1ProjectileSpeed = float.Parse(o);
            	_dic.Remove(_addID + "Skill1ProjectileSpeed");
            }
            m_Data.Add("Skill1ProjectileSpeed",Skill1ProjectileSpeed);
            if (_dic.TryGetValue(_addID + "Skill1Count", out o) && !string.IsNullOrEmpty(o))
            {
            	Skill1Count = int.Parse(o);
            	_dic.Remove(_addID + "Skill1Count");
            }
            m_Data.Add("Skill1Count",Skill1Count);
            if (_dic.TryGetValue(_addID + "Skill1CountInterval", out o) && !string.IsNullOrEmpty(o))
            {
            	Skill1CountInterval = float.Parse(o);
            	_dic.Remove(_addID + "Skill1CountInterval");
            }
            m_Data.Add("Skill1CountInterval",Skill1CountInterval);
            if (_dic.TryGetValue(_addID + "Skill2Id", out o) && !string.IsNullOrEmpty(o))
            {
            	Skill2Id = o;
            	_dic.Remove(_addID + "Skill2Id");
            }
            m_Data.Add("Skill2Id",Skill2Id);
            if (_dic.TryGetValue(_addID + "Skill2Telegraph", out o) && !string.IsNullOrEmpty(o))
            {
            	Skill2Telegraph = float.Parse(o);
            	_dic.Remove(_addID + "Skill2Telegraph");
            }
            m_Data.Add("Skill2Telegraph",Skill2Telegraph);
            if (_dic.TryGetValue(_addID + "Skill2Damage", out o) && !string.IsNullOrEmpty(o))
            {
            	Skill2Damage = int.Parse(o);
            	_dic.Remove(_addID + "Skill2Damage");
            }
            m_Data.Add("Skill2Damage",Skill2Damage);
            if (_dic.TryGetValue(_addID + "Skill2TriggerDistance", out o) && !string.IsNullOrEmpty(o))
            {
            	Skill2TriggerDistance = float.Parse(o);
            	_dic.Remove(_addID + "Skill2TriggerDistance");
            }
            m_Data.Add("Skill2TriggerDistance",Skill2TriggerDistance);
            if (_dic.TryGetValue(_addID + "Skill2Speed", out o) && !string.IsNullOrEmpty(o))
            {
            	Skill2Speed = float.Parse(o);
            	_dic.Remove(_addID + "Skill2Speed");
            }
            m_Data.Add("Skill2Speed",Skill2Speed);
            if (_dic.TryGetValue(_addID + "Skill2Interval", out o) && !string.IsNullOrEmpty(o))
            {
            	Skill2Interval = float.Parse(o);
            	_dic.Remove(_addID + "Skill2Interval");
            }
            m_Data.Add("Skill2Interval",Skill2Interval);
            if (_dic.TryGetValue(_addID + "Skill2AreaWidth", out o) && !string.IsNullOrEmpty(o))
            {
            	Skill2AreaWidth = float.Parse(o);
            	_dic.Remove(_addID + "Skill2AreaWidth");
            }
            m_Data.Add("Skill2AreaWidth",Skill2AreaWidth);
            if (_dic.TryGetValue(_addID + "Skill2AreaCount", out o) && !string.IsNullOrEmpty(o))
            {
            	Skill2AreaCount = int.Parse(o);
            	_dic.Remove(_addID + "Skill2AreaCount");
            }
            m_Data.Add("Skill2AreaCount",Skill2AreaCount);
            if (_dic.TryGetValue(_addID + "Skill2EnrageAreaCount", out o) && !string.IsNullOrEmpty(o))
            {
            	Skill2EnrageAreaCount = int.Parse(o);
            	_dic.Remove(_addID + "Skill2EnrageAreaCount");
            }
            m_Data.Add("Skill2EnrageAreaCount",Skill2EnrageAreaCount);
        }
        #endregion
    }
}
