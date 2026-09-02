using System;
using System.Collections.Generic;

namespace Library
{
    /// <summary>변경 감지/저장/로드 및 Global/Local/Control 이벤트 분리를 지원하는 반응형 값의 추상 베이스 클래스</summary>
    public abstract class ValueBase
    {
        #region Type
        /// <summary>값 변경 시 호출 유형</summary>
        public enum EChangeType
        {
            /// <summary>저장/로드 이벤트 없음</summary>
            None,
            /// <summary>저장 이벤트 호출 필요</summary>
            NeedSave,
            /// <summary>로드 완료 후 호출</summary>
            Loaded
        }
        #endregion
        #region Property
        /// <summary>값의 고유 식별자</summary>
        public string ID { get; private set; }
        #endregion
        #region Value
        protected Dictionary<ControlBase, Action<ValueBase>> m_OnControlChanged = new();
        protected Action<ValueBase> m_OnLocalChanged;
        protected Action<ValueBase> m_OnConstraintChanged;
        protected Action<ValueBase> m_OnResourceChanged;
        protected Action<ValueBase> m_OnGlobalChanged;
        protected Action<ValueBase> m_OnSaveChanged;
        protected Action<ValueBase> m_OnLoaded;
        #endregion

        #region Event
        protected ValueBase(IManageValue _callBy, string _id)
        {
            _callBy?.ManageValue(this);
            ID = _id;
        }
        /// <summary>현재 값을 저장용 객체로 반환한다</summary>
        public virtual object OnSave()
        {
            return "";
        }
        /// <summary>현재 값을 저장용 문자열로 반환한다</summary>
        public virtual string OnSaveString()
        {
            return OnSave().ToString();
        }
        /// <summary>_data 에서 값을 읽어들인다</summary>
        public virtual void OnLoad(object _data)
        {
        }
        /// <summary>_data 문자열에서 값을 읽어들인다</summary>
        public virtual void OnLoadString(string _data)
        {
        }

        /// <summary>등록된 리스너를 정해진 순서로 호출한다. _type 에 따라 로드·저장 리스너가 끼어든다</summary>
        // 호출 순서가 계약이다 — 제약 → 로드 → 리소스 → 글로벌 → 로컬 → 저장 → 컨트롤. 제약이 먼저 값을 보정한 뒤 나머지가 그 결과를 본다
        protected virtual void OnChanged(EChangeType _type)
        {
            m_OnConstraintChanged?.Invoke(this);
            if (_type == EChangeType.Loaded)
                m_OnLoaded?.Invoke(this);
            m_OnResourceChanged?.Invoke(this);
            m_OnGlobalChanged?.Invoke(this);
            m_OnLocalChanged?.Invoke(this);
            if (_type == EChangeType.NeedSave)
                m_OnSaveChanged?.Invoke(this);
            foreach (var v in m_OnControlChanged)
                if (v.Key.IsActive)
                    v.Value?.Invoke(this);
        }
        /// <summary>Local·Control 리스너를 모두 비운다 (씬을 벗어날 때 잔여 참조를 끊는 용도. Global 리스너는 남는다)</summary>
        public virtual void OnResetLocalChanged()
        {
            m_OnLocalChanged = null;
            m_OnControlChanged.Clear();
        }
        #endregion
        #region Local Function
        /// <summary>_callBy 의 타입 이름을 예외 문구용으로 만든다 (null·파괴된 오브젝트도 구분해서 돌려준다)</summary>
        private static string TypeName(object _callBy)
        {
            if (_callBy == null)
                return "null";
            if (_callBy is UnityEngine.Object obj && obj == null)
                return $"파괴된 {_callBy.GetType().Name}";
            return _callBy.GetType().Name;
        }
        #endregion
        #region Function
        /// <summary>_ctrl 이 등록한 리스너만 즉시 한 번 호출한다 (활성 여부와 무관)</summary>
        public void CallControlEvent(ControlBase _ctrl)
        {
            if (m_OnControlChanged.TryGetValue(_ctrl, out var action))
                action?.Invoke(this);
        }
        /// <summary>저장이 필요한 변경(NeedSave)에 반응할 리스너를 등록한다. _isCallNow 가 true 면 등록 즉시 _action 을 한 번 호출한다</summary>
        public void AddSaveChanged(GlobalManagerBase _callBy, Action<ValueBase> _action, bool _isCallNow = false)
        {
            m_OnSaveChanged += _action;
            if (_isCallNow)
                _action?.Invoke(this);
        }
        /// <summary>리소스 변경 리스너를 등록한다. _isCallNow 가 true 면 등록 즉시 _action 을 한 번 호출한다</summary>
        public void AddResourceChanged(GlobalManagerBase _callBy, Action<ValueBase> _action, bool _isCallNow = false)
        {
            m_OnResourceChanged += _action;
            if (_isCallNow)
                _action?.Invoke(this);
        }
        /// <summary>제약조건 리스너를 등록하고 즉시 한 번 호출한다. 제약은 다른 리스너보다 먼저 돌아 값을 보정할 수 있다</summary>
        public void AddConstraintChanged(GlobalManagerBase _callBy, Action<ValueBase> _action)
        {
            m_OnConstraintChanged += _action;
            _action?.Invoke(this);
        }
        /// <summary>값을 바꾸지 않고 _type 의 변경 이벤트만 수동으로 발생시킨다</summary>
        public void PostChanged(EChangeType _type)
        {
            OnChanged(_type);
        }
        /// <summary>변경 리스너를 등록한다. _callBy 의 타입이 등록 계열을 정한다 — 글로벌 매니저는 Global, 로컬 매니저·오브젝트는 Local, 컨트롤은 Control(비활성 시 호출되지 않는다). 그 외 타입이면 예외. _isCallNow 가 true 면 등록 즉시 _action 을 한 번 호출한다</summary>
        public void AddChanged(object _callBy, Action<ValueBase> _action, bool _isCallNow = false)
        {
            if (_callBy as GlobalManagerBase)
                m_OnGlobalChanged += _action;
            else if (_callBy as LocalManagerBase || _callBy as ObjectBase)
                m_OnLocalChanged += _action;
            else if (_callBy is ControlBase ctrl)
            {
                if (m_OnControlChanged.ContainsKey(ctrl))
                    m_OnControlChanged[ctrl] += _action;
                else
                    m_OnControlChanged.Add(ctrl, _action);
                ctrl.AddValueBase(this);
            }
            else
                throw new InvalidOperationException($"{ID} : AddChanged 의 _callBy 가 {TypeName(_callBy)} 다 — GlobalManagerBase·LocalManagerBase·ObjectBase·ControlBase 만 구독할 수 있다");

            if (_isCallNow)
                _action?.Invoke(this);
        }
        /// <summary>AddChanged 로 등록한 리스너를 제거한다. _callBy 는 등록 때와 같은 것을 넘겨야 한다</summary>
        public void RemoveChanged(object _callBy, Action<ValueBase> _action)
        {
            if (_callBy as GlobalManagerBase)
                m_OnGlobalChanged -= _action;
            else if (_callBy as LocalManagerBase || _callBy as ObjectBase)
                m_OnLocalChanged -= _action;
            else if (_callBy is ControlBase ctrl)
            {
                if (m_OnControlChanged.TryGetValue(ctrl, out var existingDelegate))
                {
                    existingDelegate -= _action;
                    if (existingDelegate == null)
                    {
                        m_OnControlChanged.Remove(ctrl);
                        if (ctrl != null)
                            ctrl.RemoveValueBase(this);
                    }
                    else
                        m_OnControlChanged[ctrl] = existingDelegate;
                }
            }
            else
                throw new InvalidOperationException($"{ID} : RemoveChanged 의 _callBy 가 {TypeName(_callBy)} 다 — 등록 때와 같은 GlobalManagerBase·LocalManagerBase·ObjectBase·ControlBase 를 넘겨야 한다");
        }
        /// <summary>로드 완료 리스너를 등록한다. _callBy 는 글로벌 매니저만 허용하며 그 외에는 예외. _isCallNow 가 true 면 등록 즉시 _action 을 한 번 호출한다</summary>
        public void AddLoadEvent(object _callBy, Action<ValueBase> _action, bool _isCallNow = false)
        {
            if (_callBy as GlobalManagerBase)
            {
                m_OnLoaded += _action;
            }
            else
                throw new InvalidOperationException($"{ID} : AddLoadEvent 의 _callBy 가 {TypeName(_callBy)} 다 — GlobalManagerBase 만 등록할 수 있다");

            if (_isCallNow)
                _action?.Invoke(this);
        }
        #endregion
    }
}