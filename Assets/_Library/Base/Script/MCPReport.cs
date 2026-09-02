#if UNITY_EDITOR
using System.Text;

namespace Library
{
    /// <summary>노출 항목을 "id → 값" 맵으로 모으는 빌더. detail·interaction·cheats 가 모두 같은 형식이라, 구현체는 JSON 을 직접 쓰지 않고 Add 로만 채운다</summary>
    public class MCPReport
    {
        #region Property
        /// <summary>추가된 항목이 없으면 true</summary>
        public bool IsEmpty => m_First;
        #endregion
        #region Value
        private readonly StringBuilder m_Builder = new();
        private bool m_First = true;
        #endregion
        #region Local Function
        /// <summary>항목 사이 쉼표와 키를 이어 붙인다</summary>
        private void AppendKey(string _id)
        {
            if (!m_First) m_Builder.Append(',');
            m_First = false;
            m_Builder.Append('"').Append(Escape(_id)).Append("\":");
        }
        #endregion
        #region Function
        /// <summary>_id 에 문자열 _value 를 담고 자신을 반환한다 (이어 쓰기용). 이스케이프·따옴표는 알아서 처리한다</summary>
        public MCPReport Add(string _id, string _value)
        {
            AppendKey(_id);
            m_Builder.Append('"').Append(Escape(_value)).Append('"');
            return this;
        }
        /// <summary>_id 에 _value 를 따옴표 없는 숫자로 담고 자신을 반환한다</summary>
        public MCPReport AddNumber(string _id, long _value)
        {
            AppendKey(_id);
            m_Builder.Append(_value);
            return this;
        }
        /// <summary>_id 에 이미 JSON 인 _rawJson 을 그대로 담고 자신을 반환한다 (객체·배열·불리언용). 이스케이프하지 않으므로 유효한 JSON 이어야 하며, 비면 null 로 들어간다</summary>
        public MCPReport AddRaw(string _id, string _rawJson)
        {
            AppendKey(_id);
            m_Builder.Append(string.IsNullOrEmpty(_rawJson) ? "null" : _rawJson);
            return this;
        }
        /// <summary>모은 항목을 JSON 객체 문자열로 반환한다 (예: {"a":"1","b":"2"})</summary>
        public string ToJson()
        {
            return "{" + m_Builder + "}";
        }
        /// <summary>_s 를 JSON 문자열에 넣을 수 있게 이스케이프한다. null·빈 값은 빈 문자열이 된다</summary>
        public static string Escape(string _s)
        {
            if (string.IsNullOrEmpty(_s)) return "";
            return _s.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r");
        }
        #endregion
    }
}
#endif
