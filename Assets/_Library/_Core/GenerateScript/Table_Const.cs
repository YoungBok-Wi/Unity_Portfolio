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
        #endregion

        #region Event
        public void Init()
        {
            var json = Resources.Load<TextAsset>("Table/TableConst");
            if (json == null) return;
            var data = (Dictionary<string, object>)MiniJson.Deserialize(json.text);
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
        }
        #endif
        #endregion
    }
}
