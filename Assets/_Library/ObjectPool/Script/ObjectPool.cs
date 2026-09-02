using System;
using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>프리팹 인스턴스를 미리 만들어 두고 Get·Return 으로 돌려 쓰는 풀. 싱글톤이 아니라 필요한 곳에서 직접 만들어 쓴다</summary>
    public class ObjectPool
    {
        #region Value
        private GameObject m_Prefab;
        private Transform m_Root;
        public Action<GameObject> m_InitFunc;
        public Action<GameObject, bool> m_SetReusableFunc;
        private Queue<GameObject> m_Pool = new();
        private List<GameObject> m_Using = new();
        #endregion

        #region Event
        /// <summary>_prefab 인스턴스를 _size 개 만들어 _root 하위에 채운다. _initFunc 은 생성 직후 1회, _setResuableFunc 은 풀에 들고 날 때마다 불리며 생략하면 활성/비활성으로 처리한다</summary>
        public ObjectPool(GameObject _prefab, Transform _root, int _size, Action<GameObject> _initFunc = null, Action<GameObject, bool> _setResuableFunc = null)
        {
            if (_prefab == null)
                throw new ArgumentNullException(nameof(_prefab));
            if (_size < 0)
                throw new ArgumentOutOfRangeException(nameof(_size));

            m_Prefab = _prefab;
            m_Root = _root;
            m_InitFunc = _initFunc;
            m_SetReusableFunc = (_setResuableFunc != null) ? _setResuableFunc : DefaultSetReusableFunc;

            // 인스턴스는 재사용 처리(위 루프)로 이미 비활성이다 — 원본을 끄면 프리팹 에셋 참조일 때 에셋 자체가 변조·커밋된다
            for (int i = 0; i < _size; ++i)
            {
                var obj = GameObject.Instantiate(m_Prefab, m_Root);
                m_Pool.Enqueue(obj);
                m_InitFunc?.Invoke(obj);
                m_SetReusableFunc(obj, true);
            }
        }
        #endregion
        #region Local Function
        /// <summary>기본 재사용 처리 — 풀에 있으면 비활성, 꺼내 쓰면 활성</summary>
        private static void DefaultSetReusableFunc(GameObject _obj, bool _isAct)
        {
            _obj.gameObject.SetActive(!_isAct);
        }
        #endregion
        #region Function
        /// <summary>쓸 오브젝트를 하나 꺼낸다. 풀이 비면 null 이다 — 늘려 주지 않으므로 호출측이 처리해야 한다</summary>
        public GameObject Get()
        {
            if (m_Pool.Count == 0)
                return null;

            var obj = m_Pool.Dequeue();
            m_Using.Add(obj);
            m_SetReusableFunc(obj, false);

            return obj;
        }
        /// <summary>다 쓴 오브젝트를 풀에 돌려놓는다. 이 풀에서 나온 게 아니면 무시하므로 중복 반환도 안전하다</summary>
        public void Return(GameObject _object)
        {
            if (m_Using.Remove(_object))
            {
                m_SetReusableFunc(_object, true);
                m_Pool.Enqueue(_object);
            }
        }
        /// <summary>쓰이던 것까지 전부 풀로 되돌린다. 오브젝트를 없애지는 않는다</summary>
        public void Clear()
        {
            for (int i = 0; i < m_Using.Count; ++i)
            {
                var o = m_Using[i];
                m_SetReusableFunc(o, true);
                m_Pool.Enqueue(o);
            }
            m_Using.Clear();
        }
        #endregion
    }
}