/* 게임 테이블 매니저 — 게임 테이블 프로퍼티, 초기화, 백엔드 로드 */
#if NBING_THEBACKEND
using BackEnd.Content;
#endif
using System.Collections.Generic;
using Library;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Library
{
    /// <summary>게임 테이블 매니저(생성 partial). 테이블 프로퍼티/초기화 담당. 싱글톤·공통 조회·로드 및 base 선언은 TableManager.cs(수기 partial)</summary>
    public partial class TableManager
    {
        #if UNITY_EDITOR
        #region Preview
        [SerializeField, ReadOnly] private List<string> m_TextPreview;
        #endregion
        #endif

        #region Property
        public Table_Const Const { get; } = new();
        public Table_Text Text { get; } = new();
        #endregion

        #region Event
        public override void Init()
        {
            Const.Init();
            Text.Init(All);
            base.Init();

            #if UNITY_EDITOR
            m_TextPreview = Text.ID as List<string>;
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
            		if (chart.TryGetValue("Text", out c))
            			Text.OnApplyBackend(chart["Text"], All);
            	}
            }
            await base.InitBackend();
        }
        #endif
        #endregion
    }
}
