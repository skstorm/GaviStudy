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

        protected override void enterState()
        {
            Debug.Log($"{Localize.Get(ETextKind.InGameScene)} Start");
            _goTitleButton.onClick.RemoveAllListeners();
            _goTitleButton.onClick.AddListener(goTitle);
        }

        private void goTitle()
        {
            var nextState = Util.LoadScenePrefab<TitleSceneState>(Const.PathTitleScenePrefab, _ownerFsm);
            _ownerFsm.ChangeState(nextState);
        }

        protected override void updateState()
        {
            Debug.Log("A Update" + count);
        }

        protected override void exitState()
        {
            Debug.Log("A End");
            Destroy(gameObject);
        }
    }
}