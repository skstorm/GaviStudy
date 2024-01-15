using UnityEngine;
using UnityEngine.UI;

namespace GameJam
{
    public class TitleSceneState : BaseStateBehaviour
    {
        private int count = 0;

        [SerializeField]
        private Text _text;

        protected override void enterState()
        {
            Debug.Log($"{Localize.Get(ETextKind.TitleScene)} Start");
        }

        protected override void updateState()
        {
            ++count;
            Debug.Log("B Update" + count);
            if (count > 10)
            {
                var nextState = Util.LoadScenePrefab<InGameSceneState>(Const.PathInGameScenePrefab, _ownerFsm);
                _ownerFsm.ChangeState(nextState);
            }
        }

        protected override void exitState()
        {
            Debug.Log("B End");
            Destroy(gameObject);
        }
    }
}
