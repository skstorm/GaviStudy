using UnityEngine;
using UnityEngine.UI;

namespace GameJam
{
    public class InGameSceneState : BaseStateBehaviour
    {
        private int count = 0;

        [SerializeField]
        private Text _text;

        protected override void enterState()
        {
            Debug.Log($"{Localize.Get(ETextKind.InGameScene)} Start");
        }

        protected override void updateState()
        {
            ++count;
            Debug.Log("A Update" + count);
            if (count > 10)
            {
                var nextState = BaseStateBehaviour.Load<TitleSceneState>("Prefabs/TitleScene", _ownerFsm);
                _ownerFsm.ChangeState(nextState);
            }
        }

        protected override void exitState()
        {
            Debug.Log("A End");
            Destroy(gameObject);
        }
    }
}