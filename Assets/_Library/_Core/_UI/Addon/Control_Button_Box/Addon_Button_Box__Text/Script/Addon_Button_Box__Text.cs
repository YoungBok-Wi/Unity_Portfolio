using UnityEngine;

namespace Library
{
    /// <summary>Control_Button_Box 라벨 텍스트 애드온. 번역 책임은 팝업(PopupBase.OnLanguageChanged)으로 이관되어, 이 컴포넌트는 애드온 설계 표준상 동명 애드온에만 남는 표시 마커이며 자체 번역 로직은 갖지 않는다.</summary>
    [RequireComponent(typeof(UIWrapper_Text))]
    public class Addon_Button_Box__Text : ControlBase
    {
    }
}
