#if NBING_THEBACKEND
using BackEnd.Content;
using LitJson;
#endif
using System.Collections.Generic;
using Library;
using UnityEngine;

namespace Library
{
    public partial class Table_Character
    {
        #region Property
        public IReadOnlyDictionary<string, CharacterTable> Data => m_Data;
        public IReadOnlyList<string> ID => m_ID;
        public int Count => m_ID.Count;
        #endregion

        #region Value
        private Dictionary<string, CharacterTable> m_Data = new();
        private List<string> m_ID = new();
        #endregion

        #region Event
        public void Init(Table_All _all)
        {
            var json = Resources.Load<TextAsset>("Table/TableCharacter");
            if (json == null) return;
            var data = (Dictionary<string, object>)MiniJson.Deserialize(json.text);
            foreach (var id in data.Keys)
            {
                var dic = new Dictionary<string, string>();
                var row = (Dictionary<string, object>)data[id];
                foreach (var key in row.Keys)
                    dic[key] = row[key].ToString();
                m_Data.Add(id, new("Character", id, "", dic));
                m_ID.Add(id);
                _all.Apply(id, m_Data[id]);
            }
        }

        #if NBING_THEBACKEND
        public void OnApplyBackend(ContentItem _chart, Table_All _all)
        {
            foreach (JsonData v in _chart.contentJson)
            {
            	var dic = new Dictionary<string, string>();
            	foreach(var w in v.Keys)
            		dic.Add(w, v[w].ToString());
            	var key = v["ItemID"].ToString();
            	var item = new CharacterTable("Character", key, "", dic);
            	if (m_Data.TryGetValue(key, out var d))
            	{
            		m_Data[key] = item;
            	}
            	else
            	{
            		m_ID.Add(v["ItemID"].ToString());
            		m_Data.Add(key, item);
            	}
            	_all.Apply(key, item);
            }
        }
        #endif
        #endregion
    }
}
