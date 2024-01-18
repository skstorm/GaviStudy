using UnityEngine;
using UnityEngine.UI;

namespace GameJam
{
    public class InGameSceneState : BaseStateBehaviour
    {
        private int count = 0;

        [SerializeField]
        private Text _text;

        [SerializeField]
        private Button _goTitleButton;

        protected override BaseStateBehaviour LoadScenePrefab()
        {
            return Util.LoadScenePrefab<InGameSceneState>(Const.PathInGameScenePrefab, _owner);
        }

        protected override void enterState()
        {
            Util.DebugLog($"{Localize.Get(ETextKind.InGameScene)} Start");
            _goTitleButton.onClick.RemoveAllListeners();
            _goTitleButton.onClick.AddListener(goTitle);
        }

        private void goTitle()
        {
            var nextState = Util.LoadScenePrefab<TitleSceneState>(Const.PathTitleScenePrefab, _owner);
            _owner.ChangeState(nextState);
        }

        protected override void updateState()
        {
            //Util.DebugLog("A Update" + count);
        }

        protected override void exitState()
        {
            Util.DebugLog("A End");
            Destroy(gameObject);
        }
    }
}