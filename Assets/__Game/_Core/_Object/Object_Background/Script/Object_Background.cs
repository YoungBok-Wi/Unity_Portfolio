using Library;
using UnityEngine;

namespace Game
{
    /// <summary>사이드뷰 배경 — 로비는 로비 배경, 게임 씬은 현재 방 종류(Battle·Heal·Ability·Boss)에 맞는 배경으로 바꾼다</summary>
    public class Object_Background : ObjectBase
    {
        #region Inspector
        [SerializeField, Tooltip("배경 렌더러")] private SpriteRenderer m_Renderer;
        [SerializeField, Tooltip("로비 배경")] private Sprite m_Lobby;
        [SerializeField, Tooltip("Battle 방 배경")] private Sprite m_Battle;
        [SerializeField, Tooltip("Heal 방 배경")] private Sprite m_Heal;
        [SerializeField, Tooltip("Ability 방 배경")] private Sprite m_Ability;
        [SerializeField, Tooltip("Boss 방 배경")] private Sprite m_Boss;
        #endregion

        #region Event
        public override void InitSingleton()
        {
            base.InitSingleton();
        }
        public override void InitGame()
        {
            var room = LocalRoomManager.instance;
            if (room != null)
                room.RoomKind.AddChanged(this, OnRoomKindChanged, true);
            else
                m_Renderer.sprite = m_Lobby;
            base.InitGame();
        }
        public override void OnShutdown()
        {
            var room = LocalRoomManager.instance;
            if (room != null)
                room.RoomKind.RemoveChanged(this, OnRoomKindChanged);
            base.OnShutdown();
        }
        /// <summary>방 종류에 맞는 배경으로 바꾼다 (방 입장 전엔 Battle)</summary>
        private void OnRoomKindChanged(ValueBase _)
        {
            switch (LocalRoomManager.instance.RoomKind.v)
            {
                case RoomConst.KindHeal: m_Renderer.sprite = m_Heal; break;
                case RoomConst.KindAbility: m_Renderer.sprite = m_Ability; break;
                case RoomConst.KindBoss: m_Renderer.sprite = m_Boss; break;
                default: m_Renderer.sprite = m_Battle; break;
            }
        }
        #endregion
    }
}
