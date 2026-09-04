#if NBING_THEBACKEND
using BackEnd.Content;
#endif
using System;
#if NBING_THEBACKEND
using LitJson;
#endif
using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    public partial class Table_Const
    {
        #region Property
        public float Room_GrowthHp { get; private set; }
        public float Room_GrowthAtk { get; private set; }
        public int Room_GunUnlock { get; private set; }
        public int Room_BossMin { get; private set; }
        public int Room_BossForce { get; private set; }
        public float Room_HealRatio { get; private set; }
        public int Battle_MaxEnemyOnScreen { get; private set; }
        public int Battle_MeleeSlotPerSide { get; private set; }
        public int Ability_RerollBaseCost { get; private set; }
        public int Ability_RerollCostStep { get; private set; }
        public int Ability_ChoiceCount { get; private set; }
        public string Room_ChoiceSet1 { get; private set; }
        public string Room_ChoiceSet2 { get; private set; }
        public string Room_ChoiceSet3 { get; private set; }
        public string Room_ChoiceSet4 { get; private set; }
        public float Battle_BossBgmPitch { get; private set; }
        #endregion

        #region Event
        public void Init()
        {
            var json = Resources.Load<TextAsset>("Table/TableConst");
            if (json == null) return;
            var data = (Dictionary<string, object>)MiniJson.Deserialize(json.text);
            if (data.ContainsKey("Room_GrowthHp"))
                Room_GrowthHp = float.Parse(((Dictionary<string, object>)data["Room_GrowthHp"])[""].ToString());
            if (data.ContainsKey("Room_GrowthAtk"))
                Room_GrowthAtk = float.Parse(((Dictionary<string, object>)data["Room_GrowthAtk"])[""].ToString());
            if (data.ContainsKey("Room_GunUnlock"))
                Room_GunUnlock = int.Parse(((Dictionary<string, object>)data["Room_GunUnlock"])[""].ToString());
            if (data.ContainsKey("Room_BossMin"))
                Room_BossMin = int.Parse(((Dictionary<string, object>)data["Room_BossMin"])[""].ToString());
            if (data.ContainsKey("Room_BossForce"))
                Room_BossForce = int.Parse(((Dictionary<string, object>)data["Room_BossForce"])[""].ToString());
            if (data.ContainsKey("Room_HealRatio"))
                Room_HealRatio = float.Parse(((Dictionary<string, object>)data["Room_HealRatio"])[""].ToString());
            if (data.ContainsKey("Battle_MaxEnemyOnScreen"))
                Battle_MaxEnemyOnScreen = int.Parse(((Dictionary<string, object>)data["Battle_MaxEnemyOnScreen"])[""].ToString());
            if (data.ContainsKey("Battle_MeleeSlotPerSide"))
                Battle_MeleeSlotPerSide = int.Parse(((Dictionary<string, object>)data["Battle_MeleeSlotPerSide"])[""].ToString());
            if (data.ContainsKey("Ability_RerollBaseCost"))
                Ability_RerollBaseCost = int.Parse(((Dictionary<string, object>)data["Ability_RerollBaseCost"])[""].ToString());
            if (data.ContainsKey("Ability_RerollCostStep"))
                Ability_RerollCostStep = int.Parse(((Dictionary<string, object>)data["Ability_RerollCostStep"])[""].ToString());
            if (data.ContainsKey("Ability_ChoiceCount"))
                Ability_ChoiceCount = int.Parse(((Dictionary<string, object>)data["Ability_ChoiceCount"])[""].ToString());
            if (data.ContainsKey("Room_ChoiceSet1"))
                Room_ChoiceSet1 = ((Dictionary<string, object>)data["Room_ChoiceSet1"])[""].ToString();
            if (data.ContainsKey("Room_ChoiceSet2"))
                Room_ChoiceSet2 = ((Dictionary<string, object>)data["Room_ChoiceSet2"])[""].ToString();
            if (data.ContainsKey("Room_ChoiceSet3"))
                Room_ChoiceSet3 = ((Dictionary<string, object>)data["Room_ChoiceSet3"])[""].ToString();
            if (data.ContainsKey("Room_ChoiceSet4"))
                Room_ChoiceSet4 = ((Dictionary<string, object>)data["Room_ChoiceSet4"])[""].ToString();
            if (data.ContainsKey("Battle_BossBgmPitch"))
                Battle_BossBgmPitch = float.Parse(((Dictionary<string, object>)data["Battle_BossBgmPitch"])[""].ToString());
        }

        #if NBING_THEBACKEND
        public void OnApplyBackend(ContentItem _chart)
        {
            var dic = new Dictionary<string, Dictionary<string, string>>();
            foreach (JsonData v in _chart.contentJson)
            {
            	var ids = v["ItemID"].ToString().Split('.');
            	if (!dic.TryGetValue(ids[0], out var d))
            		dic.Add(ids[0], d = new());
            	d.Add((1 < ids.Length) ? ids[1] : "", v["Value"].ToString());
            }
            Room_GrowthHp = float.Parse(dic["Room_GrowthHp"][""]);
            Room_GrowthAtk = float.Parse(dic["Room_GrowthAtk"][""]);
            Room_GunUnlock = int.Parse(dic["Room_GunUnlock"][""]);
            Room_BossMin = int.Parse(dic["Room_BossMin"][""]);
            Room_BossForce = int.Parse(dic["Room_BossForce"][""]);
            Room_HealRatio = float.Parse(dic["Room_HealRatio"][""]);
            Battle_MaxEnemyOnScreen = int.Parse(dic["Battle_MaxEnemyOnScreen"][""]);
            Battle_MeleeSlotPerSide = int.Parse(dic["Battle_MeleeSlotPerSide"][""]);
            Ability_RerollBaseCost = int.Parse(dic["Ability_RerollBaseCost"][""]);
            Ability_RerollCostStep = int.Parse(dic["Ability_RerollCostStep"][""]);
            Ability_ChoiceCount = int.Parse(dic["Ability_ChoiceCount"][""]);
            Room_ChoiceSet1 = dic["Room_ChoiceSet1"][""];
            Room_ChoiceSet2 = dic["Room_ChoiceSet2"][""];
            Room_ChoiceSet3 = dic["Room_ChoiceSet3"][""];
            Room_ChoiceSet4 = dic["Room_ChoiceSet4"][""];
            Battle_BossBgmPitch = float.Parse(dic["Battle_BossBgmPitch"][""]);
        }
        #endif
        #endregion
    }
}
