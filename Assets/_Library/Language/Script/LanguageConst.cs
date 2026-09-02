using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    /// <summary>지원 언어 목록과 언어별 번역 컬럼 인덱스의 정의처</summary>
    public static class LanguageConst
    {
        #region Const
        public static readonly List<SystemLanguage> LanguageList = new List<SystemLanguage>
        {
            SystemLanguage.English,
            SystemLanguage.Korean,
            SystemLanguage.Japanese
        };
        public static readonly Dictionary<SystemLanguage, int> LanguageIndex = new Dictionary<SystemLanguage, int>
        {
            { SystemLanguage.English, 0 },
            { SystemLanguage.Korean, 1 },
            { SystemLanguage.Japanese, 2 }
        };
        #endregion
    }
}
