using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>아이콘 관리 매니저, ID→Sprite 매핑 및 Table/Resources 폴백</summary>
    public class IconManager : GlobalManagerBase
    {
        public static IconManager instance { get; private set; }

        #region Preview
#if UNITY_EDITOR
        [Serializable] private struct SPreview
        {
            public string id;
            public string by;
            public Sprite icon;
            public SPreview(string _id, string _by, Sprite _icon)
            {
                id = _id;
                by = _by;
                icon = _icon;
            }
        }
        [SerializeField, TabGroup("IconManager", "미리보기"), ReadOnly] private List<SPreview> m_Preview = new();
#endif
        #endregion
        #region Value
        private Dictionary<string, Sprite> m_Icon = new Dictionary<string, Sprite>();
        private Dictionary<string, ValueBase> m_Value = new Dictionary<string, ValueBase>();
        private List<Sprite> m_Exported;
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        #endregion
        #region Local Function
        /// <summary>Resources "Icon" 폴더에서 _name 의 스프라이트를 찾아 반환한다. 경로 전체일치를 먼저 보고, 없으면 반출명 규약 "{타입ID}_{파일ID}" 의 파일 ID 로 보아 "_{_name}" 으로 끝나는 항목을 찾는다. 없거나 후보가 둘 이상이면 null</summary>
        private Sprite LoadExported(string _name)
        {
            var exact = Resources.Load<Sprite>($"Icon/{_name}");
            if (exact != null)
                return exact;

            // 단순화: 반출 목록을 첫 조회 때 한 번만 담고 갱신하지 않으며 접미 대조도 전수 순회다. 아이콘이 수백 건이 되거나 런타임에 늘어나면 반출명 → 파일 ID 인덱스를 따로 만들어야 한다
            if (m_Exported == null)
                m_Exported = new List<Sprite>(Resources.LoadAll<Sprite>("Icon"));

            string suffix = $"_{_name}";
            Sprite matched = null;
            foreach (var sprite in m_Exported)
            {
                if (!sprite.name.EndsWith(suffix, StringComparison.Ordinal))
                    continue;
                if (matched != null)
                {
                    Debug.LogError($"반출명 접미가 겹친다 : {_name} (Resources \"Icon/\" 에 \"{suffix}\" 로 끝나는 항목이 둘 이상이라 타입 ID 를 붙인 전체 반출명으로 불러야 한다)");
                    return null;
                }
                matched = sprite;
            }

            return matched;
        }
        #endregion
        #region Function
        /// <summary>_id 아이콘을 등록한다. _value 를 주면 그 값이 바뀔 때마다 _onChanged 로 스프라이트를 다시 고르고, null 이면 등록 시 1회만 고른다. _callBy 는 초기화 전이어야 한다</summary>
        public void Create(GlobalManagerBase _callBy, string _id, ValueBase _value, Func<ValueBase, Sprite> _onChanged)
        {
            if (_callBy == null)
                throw new ArgumentNullException(nameof(_callBy), $"아이콘 등록 호출자가 null : {_id}");
            if (_callBy.IsInited)
                throw new InvalidOperationException($"초기화가 끝난 매니저는 아이콘을 등록할 수 없다 : {_callBy.name} / {_id}");

#if UNITY_EDITOR
            int index = m_Preview.Count;
            m_Preview.Add(new(_id, _callBy.name, null));
#endif

            m_Icon.Add(_id, null);
            m_Value.Add(_id, _value);
            if (_value != null)
                _value.AddResourceChanged(this, (_value) =>
                {
                    m_Icon[_id] = _onChanged(_value);

#if UNITY_EDITOR
                    m_Preview[index] = new(_id, _callBy.name, m_Icon[_id]);
#endif
                }, true);
            else
            {
                m_Icon[_id] = _onChanged(_value);
#if UNITY_EDITOR
                m_Preview[index] = new(_id, _callBy.name, m_Icon[_id]);
#endif
            }
        }
        /// <summary>_id 의 아이콘을 매니저 유무와 무관하게 반환한다. 매니저가 없으면 null, 있으면 Get(_id) 결과다 (소비처가 instance 널을 각자 막지 않게 하는 통로)</summary>
        public static Sprite GetIcon(string _id)
        {
            return instance != null ? instance.Get(_id) : null;
        }
        /// <summary>_id 의 아이콘을 반환한다. 등록분 → 테이블 → Resources 반출명 순으로 찾으며, 어디에도 없으면 찾은 경로를 LogError 로 남기고 null</summary>
        public Sprite Get(string _id)
        {
            if (m_Icon.TryGetValue(_id, out var icon) && !_id.Contains("."))
            {
                if (icon == null)
                    Debug.LogError($"등록된 아이콘이 null 이다 : {_id} (등록 콜백이 스프라이트를 찾지 못했다)");
                return icon;
            }

            string tableID = null;
            if (TableManager.instance.TryGet<object>(_id, out var table))
            {
                if (table is string iconID)
                    tableID = iconID;
                else if (table is ITableType iTable && iTable["Icon"] is string tableIconID)
                    tableID = tableIconID;
            }

            Sprite sprite = tableID != null ? LoadExported(tableID) : null;
            if (sprite == null)
                sprite = Resources.Load<Sprite>($"Icon/Icon_{_id}");
            if (sprite == null)
                sprite = LoadExported(_id);
            if (sprite == null)
                Debug.LogError($"아이콘을 찾지 못했다 : {_id} (등록 {m_Icon.Count}건에 없음 · 테이블 {(tableID == null ? "미매칭" : tableID)} · Resources \"Icon/Icon_{_id}\"·\"Icon/{_id}\" 없음 · 반출명 접미 \"_{_id}\" 없음)");

            return sprite;
        }
        /// <summary>_id 에 연동된 ValueBase 를 반환한다 (아이콘이 값에 따라 바뀔 때 구독용). 없으면 null</summary>
        public ValueBase GetValue(string _id)
        {
            if (m_Value.TryGetValue(_id, out var v))
                return v;
            return null;
        }
        /// <summary>_ids 각각의 ValueBase 를 같은 순서로 반환한다. 연동이 없는 항목은 null 로 남는다</summary>
        public ValueBase[] GetValues(string[] _ids)
        {
            ValueBase[] values = new ValueBase[_ids.Length];
            for (int i = 0; i < _ids.Length; i++)
                values[i] = GetValue(_ids[i]);

            return values;
        }
        #endregion
    }
}
