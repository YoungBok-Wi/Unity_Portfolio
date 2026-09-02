using System;
using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>Global·Local 이 공유하는 매니저 초기화 베이스. 의존 순서를 풀어 가며 단계별로 가동한다</summary>
    public class Root : MonoBehaviour
    {
        #region Manual Function
        /// <summary>_require 를 만족하는 매니저부터 _init 을 돌리기를 반복해 전부 초기화한다. 실패하면 셧다운시키고 예외를 다시 던진다</summary>
        // 매니저 간 선행 의존이 있어, 준비된 것부터 처리하는 루프를 아무도 못 줄일 때까지 돌린다.
        // 한 바퀴에 하나도 못 줄이면 순환·미충족 의존이므로 예외로 끊는다. 이름 정렬은 실행 순서를 재현 가능하게 만든다
        protected void InitManagersBase<T>(string _system, string _initName, T[] _managers, Func<T, bool> _require, Action<T> _init) where T : MonoBehaviour
        {
            List<T> manager = null;
            T current = null;
            int loop = 0;

            try
            {
                manager = new List<T>(_managers);
                manager.Sort((a, b) => string.Compare(a.name, b.name, StringComparison.OrdinalIgnoreCase));

                var remove = new List<T>();
                var lastedCount = manager.Count;
                for (; ; loop++)
                {
                    foreach (var v in manager)
                        if (_require(v))
                        {
                            LogManager.instance.Log(_system, $"loop{loop + 1} : {v.GetType()}.{_initName}()");
                            current = v;
                            _init(v);
                            current = null;
                            remove.Add(v);
                        }
                    foreach (var v in remove)
                        manager.Remove(v);

                    if (lastedCount == manager.Count)
                        throw new InvalidOperationException();
                    if (manager.Count <= 0)
                        break;

                    lastedCount = manager.Count;
                    remove.Clear();
                }
            }
            catch (Exception e)
            {
                string detail;
                if (current != null)
                    detail = $"loop{loop + 1} : [{current.name}] {current.GetType().Name}.{_initName}() failed";
                else
                {
                    string lefts = "";
                    foreach (var v in manager)
                        lefts += v.name + ", ";
                    detail = $"loop{loop + 1} : {_initName}() failed\n{lefts}";
                }

                ShutdownManager.instance.Shutdown(_system, e, detail);
                throw;
            }
        }

        /// <summary>매니저들의 instance 를 세운다. 오브젝트 자가 등록보다 먼저 끝나야 한다</summary>
        protected void InitSingletons<T>(string _system, T[] _managers) where T : ManagerBase
        {
            foreach (var v in _managers)
            {
                Debug.Log($"[{_system}] {v.GetType()}.InitSingleton()");
                v.InitSingleton();
            }
        }

        /// <summary>매니저들의 Init 을 의존 순서대로 돌린다</summary>
        protected void InitManagers<T>(string _system, T[] _managers) where T : ManagerBase
        {
            InitManagersBase(_system, "Init", _managers, (v) => v.RequireInit(), (v) => v.Init());
        }

        /// <summary>매니저들의 InitUI 를 순서 상관없이 돌린다</summary>
        protected void AfterInitManagers<T>(string _system, T[] _managers) where T : ManagerBase
        {
            foreach (var v in _managers)
            {
                Debug.Log($"[{_system}] {v.GetType()}.InitUI()");
                v.InitUI();
            }
        }

        /// <summary>매니저들의 InitGame 을 의존 순서대로 돌린다</summary>
        protected void InitGameManagers<T>(string _system, T[] _managers) where T : ManagerBase
        {
            InitManagersBase(_system, "InitGame", _managers, (v) => v.RequireInitGame(), (v) => v.InitGame());
        }
        #endregion
    }
}