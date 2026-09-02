using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>ValueBase 를 PlayerPrefs 에 자동 저장·로드하는 매니저. 기기 로컬 저장이라 계정을 따라가지 않는다</summary>
    public class PlayerPrefsSaveManager : GlobalManagerBase
    {
        public static PlayerPrefsSaveManager instance { get; private set; }

        #region Value
        private Dictionary<string, ValueBase> m_Value = new();
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        #endregion
        #region Function
        /// <summary>_value 를 저장 대상으로 등록하고, 저장돼 있던 값이 있으면 즉시 읽어 채운다. 이후 값이 바뀔 때마다 자동 저장된다</summary>
        public T Create<T>(GlobalManagerBase _callBy, T _value) where T : ValueBase
        {
            m_Value.Add(_value.ID, _value);

            var text = PlayerPrefs.GetString(_value.ID, null);
            _value.OnLoadString(text);
            _value.AddSaveChanged(this, (_v) =>
            {
                PlayerPrefs.SetString(_v.ID, _v.OnSaveString());
            }, text == null);

            return _value;
        }
#if UNITY_EDITOR
        /// <summary>등록된 저장 대상 전체 (에디터 확인용)</summary>
        public Dictionary<string, ValueBase> GetValues()
        {
            return m_Value;
        }
#endif
        #endregion
    }
}