using System.Collections.Generic;

namespace Library
{
    /// <summary>씬 로컬 매니저 베이스 클래스, 씬 전환 시 파괴, Init→InitGame 순 초기화. 하위에 둘 필요 없이 자유롭게 배치된 ObjectBase가 자가 등록하면 라이프사이클을 전파한다</summary>
    public abstract class LocalManagerBase : ManagerBase
    {
        #region Value
        protected List<ObjectBase> m_Objects = new();
        #endregion

        #region Event
        /// <summary>_object 가 자가 등록할 때 호출된다. 늦게 등록돼도 이미 지난 초기화 단계를 그 자리에서 따라잡는다. 구현체는 base 를 호출한 뒤 필요한 타입으로 걸러 쓴다</summary>
        public virtual void OnRegisterObject(ObjectBase _object)
        {
            m_Objects.Add(_object);
            _object.IsRegistered = true;
            if (IsInited)
            {
                _object.Init();
                _object.InitUIOnce();
                _object.InitUI();
            }
            if (IsGameInited)
            {
                _object.InitGame();
                _object.InitUI();
            }
        }
        public override void Init()
        {
            foreach (var v in m_Objects)
                v.Init();
            base.Init();
        }
        public override void InitUIOnce()
        {
            foreach (var v in m_Objects)
                v.InitUIOnce();
            base.InitUIOnce();
        }
        public override void InitUI()
        {
            foreach (var v in m_Objects)
                v.InitUI();
            base.InitUI();
        }
        public override void InitGame()
        {
            foreach (var v in m_Objects)
                v.InitGame();
            base.InitGame();
        }
        public override void OnShutdown()
        {
            foreach (var v in m_Objects)
                v.OnShutdown();
            base.OnShutdown();
        }
        #endregion
        #region MCP
#if UNITY_EDITOR
        /// <summary>노출할 항목을 _entries 에 담는다. 기본은 자신을 한 항목으로 담으며, detail·interaction 이 둘 다 비면 담지 않아 노출에서 빠진다. 관리 대상을 자신 대신 평평하게 노출하려면 override 한다</summary>
        public virtual void MCPCollect(System.Collections.Generic.List<SMCPEntry> _entries, string _name)
        {
            var detail = new MCPReport();
            MCPDetail(detail);
            var interaction = new MCPReport();
            MCPInteraction(interaction);
            if (detail.IsEmpty && interaction.IsEmpty) return;
            _entries.Add(new SMCPEntry(_name, detail.ToJson(), interaction.ToJson()));
        }
        /// <summary>현재 상태를 _report 에 담는다. 기본은 관리 중인 ValueBase 를 직렬화하며, 매니저별로 override 해서 상태를 직접 적는다</summary>
        public virtual void MCPDetail(MCPReport _report)
        {
            foreach (var v in m_ManageValue)
            {
                var val = v.OnSaveString();
                if (val != null && val.Length > 0 && (val[0] == '{' || val[0] == '['))
                    _report.AddRaw(v.ID, val);
                else
                    _report.Add(v.ID, val);
            }
        }
        /// <summary>지금 가능한 조작을 _report 에 "interactionId → 설명"으로 담는다. 기본은 없으며, 매니저별로 override 해서 나열한다</summary>
        public virtual void MCPInteraction(MCPReport _report)
        {
        }
        /// <summary>_interactionId 조작을 실행하고 결과 JSON 을 반환한다. _value 는 슬라이더 등 값 입력용이다. MCPInteraction 에 나열한 조작을 매니저별로 override 해서 처리한다</summary>
        public virtual string MCPInteract(string _interactionId, float _value)
        {
            return $"{{\"error\":\"지원하지 않는 조작: {MCPReport.Escape(_interactionId)}\"}}";
        }
        /// <summary>_go 가 지금 화면에 보이는지 판정한다 (활성 + Renderer 가시성). 씬 오브젝트를 나열할 때 보이는 것만 걸러내는 용도다</summary>
        protected static bool MCPIsOnScreen(UnityEngine.GameObject _go)
        {
            if (_go == null || !_go.activeInHierarchy) return false;
            foreach (var renderer in _go.GetComponentsInChildren<UnityEngine.Renderer>())
                if (renderer.isVisible) return true;
            return false;
        }
#endif
        #endregion
    }
}
