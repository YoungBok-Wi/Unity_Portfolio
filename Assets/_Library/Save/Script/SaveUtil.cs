using System;

namespace Library
{
    /// <summary>ValueBase 저장 방식 팩토리 (PlayerPrefs/DB/Cloud 선택). DB·Cloud는 PlayerPrefs 로 폴백한다</summary>
    public static class SaveUtil
    {
        #region Type
        public enum EType
        {
            None,
            PlayerPrefs,
            DB,
            Cloud,
        }
        #endregion
        #region Function
        /// <summary>_value 를 _type 저장에 연결해 반환한다. None 이면 연결 없이 그대로 돌려주고, DB·Cloud 는 PlayerPrefs 로 폴백된다</summary>
        public static T Create<T>(GlobalManagerBase _callBy, string _containerName, T _value, EType _type) where T : ValueBase
        {
            switch (_type)
            {
                case EType.None:
                    return _value;
                case EType.PlayerPrefs:
                    return PlayerPrefsSaveManager.instance.Create(_callBy, _value);
                case EType.DB:
                    return PlayerPrefsSaveManager.instance.Create(_callBy, _value);
                case EType.Cloud:
                    return PlayerPrefsSaveManager.instance.Create(_callBy, _value);
                default:
                    throw new ArgumentOutOfRangeException(nameof(_type));
            }
        }
        /// <summary>_value 를 배열 컬럼의 한 칸에 연결해 반환한다. PlayerPrefs 로 폴백되므로 배열 인자는 무시된다</summary>
        public static TValue CreateArray<TValue, TReturn>(GlobalManagerBase _callBy, string _tableName, string _arrayName, int _index, TValue _value, Action<TReturn> _onLoad, Func<TReturn> _onSave, TReturn _default, EType _type) where TValue : ValueBase
        {
            switch (_type)
            {
                case EType.None:
                    return _value;
                case EType.PlayerPrefs:
                    return PlayerPrefsSaveManager.instance.Create(_callBy, _value);
                case EType.DB:
                    return PlayerPrefsSaveManager.instance.Create(_callBy, _value);
                case EType.Cloud:
                    return PlayerPrefsSaveManager.instance.Create(_callBy, _value);
                default:
                    throw new ArgumentOutOfRangeException(nameof(_type));
            }
        }
        #endregion
    }
}
