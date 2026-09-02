using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>계층에서 ObjectBase 를 찾는 유틸리티</summary>
    public static class ObjectUtil
    {
        #region Local Function
        /// <summary>ObjectBase 를 만날 때까지 내려가며 모은다</summary>
        // ObjectBase 를 찾으면 그 아래로는 들어가지 않는다 — 중첩된 ObjectBase 는 바깥쪽이 알아서 관리한다
        private static void collectFrom(Transform _parent, List<ObjectBase> _list)
        {
            foreach (Transform child in _parent)
            {
                var obj = child.GetComponent<ObjectBase>();
                if (obj)
                    _list.Add(obj);
                else
                    collectFrom(child, _list);
            }
        }
        #endregion
        #region Function
        /// <summary>_root 아래에서 가장 바깥쪽 ObjectBase 들을 모아 반환한다. 중간에 ObjectBase 가 없는 경로는 더 내려가서 찾는다</summary>
        public static ObjectBase[] FindDirectChildren(Transform _root)
        {
            var list = new List<ObjectBase>();
            collectFrom(_root, list);
            return list.ToArray();
        }
        #endregion
    }
}
