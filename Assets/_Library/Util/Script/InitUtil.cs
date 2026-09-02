using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>선행 매니저가 준비됐는지 확인하는 유틸리티. RequireInit 계열 구현에서 쓴다</summary>
    public static class InitUtil
    {
        #region Function
        /// <summary>_mgrs 가 모두 Init 을 마쳤는지 반환한다 (빈 배열이면 true)</summary>
        public static bool IsInit<T>(T[] _mgrs) where T : ManagerBase
        {
            foreach (var v in _mgrs)
                if (!v.IsInited)
                    return false;
            return true;
        }
        /// <summary>_mgrs 가 모두 백엔드 초기화를 마쳤는지 반환한다 (빈 배열이면 true)</summary>
        public static bool IsInitBackend(GlobalManagerBase[] _mgrs)
        {
            foreach (var v in _mgrs)
                if (!v.IsBackendInited)
                    return false;
            return true;
        }
        /// <summary>_mgrs 가 모두 InitGame 을 마쳤는지 반환한다 (빈 배열이면 true)</summary>
        public static bool IsInitGame<T>(T[] _mgrs) where T : ManagerBase
        {
            foreach (var v in _mgrs)
                if (!v.IsGameInited)
                    return false;
            return true;
        }
        #endregion
    }
}