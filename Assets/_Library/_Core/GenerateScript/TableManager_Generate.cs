/* 게임 테이블 매니저 — 게임 테이블 프로퍼티, 초기화, 백엔드 로드 */
#if NBING_THEBACKEND
using BackEnd.Content;
#endif
using System.Collections.Generic;
using Library;
using UnityEngine;

namespace Library
{
    /// <summary>게임 테이블 매니저(생성 partial). 테이블 프로퍼티/초기화 담당. 싱글톤·공통 조회·로드 및 base 선언은 TableManager.cs(수기 partial)</summary>
    public partial class TableManager
    {
        #if UNITY_EDITOR
        #region Preview
        [SerializeField] private List<string> m_AbilityPreview;
        [SerializeField] private List<string> m_BossPreview;
        [SerializeField] private List<string> m_CharacterPreview;
        [SerializeField] private List<string> m_EnemyPreview;
        [SerializeField] private List<string> m_RoomPreview;
        [SerializeField] private List<string> m_TextPreview;
        [SerializeField] private List<string> m_WavePreview;
        #endregion
        #endif

        #region Property
        public Table_Const Const { get; } = new();
        public Table_Ability Ability { get; } = new();
        public Table_Boss Boss { get; } = new();
        public Table_Character Character { get; } = new();
        public Table_Enemy Enemy { get; } = new();
        public Table_Room Room { get; } = new();
        public Table_Text Text { get; } = new();
        public Table_Wave Wave { get; } = new();
        #endregion

        #region Event
        public override void Init()
        {
            Const.Init();
            Ability.Init(All);
            Boss.Init(All);
            Character.Init(All);
            Enemy.Init(All);
            Room.Init(All);
            Text.Init(All);
            Wave.Init(All);
            base.Init();

            #if UNITY_EDITOR
            m_AbilityPreview = Ability.ID as List<string>;
            m_BossPreview = Boss.ID as List<string>;
            m_CharacterPreview = Character.ID as List<string>;
            m_EnemyPreview = Enemy.ID as List<string>;
            m_RoomPreview = Room.ID as List<string>;
            m_TextPreview = Text.ID as List<string>;
            m_WavePreview = Wave.ID as List<string>;
            #endif
        }

        #if NBING_THEBACKEND
        public override async Awaitable InitBackend()
        {
            if (ThebackendManager.instance.IsLogin.v)
            {
            	var bro = await LoadAsync();
            	if (bro.IsSuccess())
            	{
            		var chart = bro.GetContentDictionarySortByChartId();
            		ContentItem c = null;
            		if (chart.TryGetValue("Const", out c))
            			Const.OnApplyBackend(chart["Const"]);
            		if (chart.TryGetValue("Ability", out c))
            			Ability.OnApplyBackend(chart["Ability"], All);
            		if (chart.TryGetValue("Boss", out c))
            			Boss.OnApplyBackend(chart["Boss"], All);
            		if (chart.TryGetValue("Character", out c))
            			Character.OnApplyBackend(chart["Character"], All);
            		if (chart.TryGetValue("Enemy", out c))
            			Enemy.OnApplyBackend(chart["Enemy"], All);
            		if (chart.TryGetValue("Room", out c))
            			Room.OnApplyBackend(chart["Room"], All);
            		if (chart.TryGetValue("Text", out c))
            			Text.OnApplyBackend(chart["Text"], All);
            		if (chart.TryGetValue("Wave", out c))
            			Wave.OnApplyBackend(chart["Wave"], All);
            	}
            }
            await base.InitBackend();
        }
        #endif
        #endregion
    }
}
