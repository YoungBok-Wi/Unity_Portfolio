using UnityEngine;

namespace Library
{
    /// <summary>표시용 문자열 변환 확장 메서드 모음</summary>
    public static class StringUtil
    {
        #region Function
        /// <summary>숫자를 표시용 문자열로 바꾼다. _isUnit 이 true 면 10000 이상부터 단위를 붙이고(예: 10000 → 10.0K), 그 미만이거나 false 면 천 단위 쉼표만 넣는다</summary>
        public static string ToStringLong(this long _number, bool _isUnit)
        {
            if (_isUnit)
            {
                if (_number < 10000)
                    return _number.ToString("N0");

                int unitIndex = (int)(Mathf.Log10(_number) / 3);
                unitIndex = Mathf.Min(unitIndex, UtilConst.m_Units.Length - 1);

                double value = _number / Mathf.Pow(1000, unitIndex);

                return $"{value:F1}{UtilConst.m_Units[unitIndex]}";
            }
            else
                return _number.ToString("N0");
        }
        #endregion
    }
}
