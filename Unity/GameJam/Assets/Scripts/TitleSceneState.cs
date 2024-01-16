using UnityEngine;
using UnityEngine.UI;

namespace GameJam
{
    public class TitleSceneState : BaseStateBehaviour
    {
        private int count = 0;

        [SerializeField]
        private Text _text;

        [SerializeField]
        private Button _goInGameButton;

        protected override BaseStateBehaviour LoadScenePrefab()
        {
            return Util.LoadScenePrefab<TitleSceneState>(Const.PathTitleScenePrefab, _owner);
        }

        protected override void enterState()
        {
            Util.DebugLog($"{Localize.Get(ETextKind.TitleScene)} Start");
            _goInGameButton.onClick.RemoveAllListeners();
            _goInGameButton.onClick.AddListener(goInGame);
        }

        private void goInGame()
        {
            var nextState = Util.LoadScenePrefab<InGameSceneState>(Const.PathInGameScenePrefab, _owner);
            _owner.ChangeState(nextState);
        }

        protected override void updateState()
        {
            Util.DebugLog("B Update" + count);
        }

        protected override void exitState()
        {
            Util.DebugLog("B End");
            Destroy(gameObject);
        }
    }
}
