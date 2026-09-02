using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>씬 오브젝트 로직의 베이스 클래스. Awake에서 InitSingleton으로 소속 매니저에 자가 등록하고, 매니저가 라이프사이클을 전파한다</summary>
    public abstract class ObjectBase : MonoBehaviour, IManageValue
    {
        #region Property
        /// <summary>Init 이 끝났는지 여부</summary>
        public bool IsInited { get; private set; }

        /// <summary>라이프사이클 전파자(매니저 또는 Local)에 등록됐는지 여부</summary>
        public bool IsRegistered { get; internal set; }
        #endregion
        #region Value
        protected List<ValueBase> m_ManageValue = new();
        #endregion

        #region Event
        protected virtual void Awake()
        {
            InitSingleton();
        }
        /// <summary>소속 매니저에 자기를 등록한다. 파생은 `{Manager}.instance.OnRegisterObject(this)` 를 부른 뒤 base 를 부르면 되고, 소속 매니저가 없으면 base 만으로 Local 이 라이프사이클을 전파한다</summary>
        public virtual void InitSingleton()
        {
            if (IsRegistered || Local.instance == null) return;
            Local.instance.OnRegisterObject(this);
        }
        public virtual void Init()
        {
            IsInited = true;
        }
        public virtual void InitUIOnce()
        {
        }
        public virtual void InitUI()
        {
        }
        public virtual void InitGame()
        {
        }
        public virtual void OnShutdown()
        {
        }
        #endregion
        #region Manual Function
        void IManageValue.ManageValue(ValueBase _value)
        {
            m_ManageValue.Add(_value);
        }
        #endregion
    }
}
