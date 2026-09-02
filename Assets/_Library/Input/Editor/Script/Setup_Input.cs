/* Input 셋업 — [Local] 하위에 [LocalInputManager] 생성, PlayerInput·EventSystem·InputSystemUIInputModule 설정 */
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEditor;

namespace Library
{
    /// <summary>[LocalInputManager]를 [Local] 하위에 생성하고 PlayerInput, EventSystem, InputSystemUIInputModule을 설정한다</summary>
    public class Setup_Input : ModuleSetupBase
    {
        #region Property
        public override string SetupName => "LocalInputManager";
        /// <summary>Popup 셋업이 만드는 UICamera 를 참조하므로 그 뒤에 실행해야 한다</summary>
        public override string[] RequireSetups => new[] { "LocalPopupManager" };
        #endregion
        #region Function
        protected override void OnSetupLocal(GameObject _root)
        {
            var go = FindOrCreateManager<LocalInputManager>(_root, "[LocalInputManager]");
            if (go == null) return;

            var actionsPath = "Assets/__Game/_Core/Setting/InputSystem_Actions.inputactions";

            // PlayerInput 이 이 오브젝트를 참조하므로 PlayerInput 보다 먼저 만든다
            var eventSystemGo = FindOrCreateChild(go, "EventSystem");
            if (eventSystemGo == null) return;

            if (eventSystemGo.GetComponent<EventSystem>() == null)
                Undo.AddComponent<EventSystem>(eventSystemGo);

            var inputModule = eventSystemGo.GetComponent<InputSystemUIInputModule>();
            if (inputModule == null)
                inputModule = Undo.AddComponent<InputSystemUIInputModule>(eventSystemGo);

            SetAssetReference(inputModule, "m_ActionsAsset", actionsPath);
            SetInputActionReference(inputModule, "m_PointAction", actionsPath, "UI/Point");
            SetInputActionReference(inputModule, "m_MoveAction", actionsPath, "UI/Navigate");
            SetInputActionReference(inputModule, "m_SubmitAction", actionsPath, "UI/Submit");
            SetInputActionReference(inputModule, "m_CancelAction", actionsPath, "UI/Cancel");
            SetInputActionReference(inputModule, "m_LeftClickAction", actionsPath, "UI/Click");
            SetInputActionReference(inputModule, "m_MiddleClickAction", actionsPath, "UI/MiddleClick");
            SetInputActionReference(inputModule, "m_RightClickAction", actionsPath, "UI/RightClick");
            SetInputActionReference(inputModule, "m_ScrollWheelAction", actionsPath, "UI/ScrollWheel");
            SetInputActionReference(inputModule, "m_TrackedDevicePositionAction", actionsPath, "UI/TrackedDevicePosition");
            SetInputActionReference(inputModule, "m_TrackedDeviceOrientationAction", actionsPath, "UI/TrackedDeviceOrientation");

            var playerInput = go.GetComponent<PlayerInput>();
            if (playerInput == null)
                playerInput = Undo.AddComponent<PlayerInput>(go);

            SetAssetReference(playerInput, "m_Actions", actionsPath);
            SetProperty(playerInput, "m_NotificationBehavior", "3");
            SetObjectReference(playerInput, "m_UIInputModule", "EventSystem (InputSystemUIInputModule)");
            SetObjectReference(playerInput, "m_Camera", "UICamera (Camera)");
        }
        #endregion
        #region Local Function
        /// <summary>"맵/액션" 경로에 해당하는 InputActionReference 를 찾아 프로퍼티에 물린다</summary>
        // InputActionReference 는 .inputactions 의 서브에셋이라, 전부 로드해 경로가 맞는 것을 골라야 한다
        private static void SetInputActionReference(Component _component, string _propertyName, string _assetPath, string _actionPath)
        {
            var so = new SerializedObject(_component);
            var prop = so.FindProperty(_propertyName);
            if (prop == null)
            {
                Debug.LogError($"[ModuleSetup] 프로퍼티를 찾을 수 없다: {_component.GetType().Name}.{_propertyName}");
                return;
            }

            var allAssets = AssetDatabase.LoadAllAssetsAtPath(_assetPath);
            foreach (var asset in allAssets)
            {
                if (asset is InputActionReference actionRef && actionRef.action != null)
                {
                    var fullPath = actionRef.action.actionMap.name + "/" + actionRef.action.name;
                    if (fullPath == _actionPath)
                    {
                        prop.objectReferenceValue = actionRef;
                        so.ApplyModifiedProperties();
                        return;
                    }
                }
            }

            Debug.LogWarning($"[ModuleSetup] InputActionReference를 찾을 수 없다: {_assetPath}:{_actionPath}");
        }
        #endregion
    }
}
