using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>씬 전환 연출의 베이스. 파생은 StartAni 로 화면을 가린 뒤 PostChange 로 씬 로드를 요청하고, EndAni 로 화면을 걷은 뒤 PostEnd 로 마무리를 알린다</summary>
    public abstract class SceneChangeAni : MonoBehaviour
    {
        #region Event
        /// <summary>연출을 초기 상태(비활성)로 되돌린다</summary>
        public virtual void Init()
        {
            gameObject.SetActive(false);
        }
        #endregion
        #region Manual Function
        /// <summary>화면을 가리는 연출을 시작한다. 파생은 연출이 끝나면 PostChange 를 불러야 한다</summary>
        public virtual void StartAni()
        {
            gameObject.SetActive(true);
        }
        /// <summary>새 씬이 올라온 뒤 화면을 걷는 연출을 시작한다. 파생은 연출이 끝나면 PostEnd 를 불러야 한다</summary>
        public virtual void EndAni()
        {
        }

        /// <summary>화면이 가려졌으니 씬을 로드해도 된다고 알린다</summary>
        protected void PostChange()
        {
            SceneChangeManager.instance.OnChange();
        }
        /// <summary>연출이 모두 끝났음을 알리고 자신을 감춘다</summary>
        protected void PostEnd()
        {
            gameObject.SetActive(false);
            SceneChangeManager.instance.OnEnd();
        }
        #endregion
    }
}