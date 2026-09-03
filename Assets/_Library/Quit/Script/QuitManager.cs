using System;
using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>앱 종료 요청을 가로채 각 시스템에 물어보는 매니저. 하나라도 거부하면 종료되지 않는다</summary>
    public class QuitManager : GlobalManagerBase
    {
        public static QuitManager instance { get; private set; }
        #region Value
        private Dictionary<GlobalManagerBase, Func<bool>> m_QuitFunc = new();
        #endregion

        #region Event
        public override void InitSingleton()
        {
            instance = this;
            base.InitSingleton();
        }
        public override void Init()
        {
            Application.wantsToQuit += OnWantsToQuit;
            base.Init();
        }

        /// <summary>등록된 모두에게 물어 종료해도 되는지 판정한다</summary>
        // 하나가 거부해도 나머지를 마저 부른다 — 각자 종료 전 정리를 할 기회를 준다
        private bool OnWantsToQuit()
        {
            bool result = true;
            foreach(var v in m_QuitFunc)
            {
                bool r = v.Value.Invoke();
                if (!r)
                    result = false;
            }

            return result;
        }
        #endregion
        #region Function
        /// <summary>앱 종료를 요청한다. 등록된 콜백들이 거부하면 실제로 종료되지 않으며, 에디터에서는 아무 일도 일어나지 않는다</summary>
        public void Quit()
        {
            Application.Quit();
        }
        /// <summary>_callBy 몫의 종료 확인 콜백을 등록한다. func 이 false 를 돌려주면 종료가 막히며, 같은 _callBy 로 다시 부르면 덮어쓴다</summary>
        public void SetFunc(GlobalManagerBase _callBy, Func<bool> func)
        {
            m_QuitFunc.Set(_callBy, func);
        }
        #endregion
    }
}