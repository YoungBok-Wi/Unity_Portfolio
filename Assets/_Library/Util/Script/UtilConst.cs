namespace Library
{
    /// <summary>Util 모듈의 코드 상수 모음</summary>
    public static class UtilConst
    {
        #region Const
        /// <summary>숫자 표시용 단위 접미사. 인덱스가 1000의 거듭제곱 자리에 대응한다 (0=단위 없음, 1=K, 2=M …)</summary>
        public static readonly string[] m_Units = { "", "K", "M", "B", "T", "P", "E" };
        #endregion
    }
}
