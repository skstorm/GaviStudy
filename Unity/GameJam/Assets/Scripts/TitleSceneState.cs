using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameJam
{
    public class TitleSceneState : BaseState
    {
        private int count = 0;
        public TitleSceneState(IBaseFsm fsm) : base(fsm)
        {
        }

        protected override void enterState()
        {
            SceneManager.LoadScene("InGameScene");
            Debug.Log("B Start");
        }
        protected override void updateState()
        {
            ++count;
            Debug.Log("B Update" + count);
            if (count > 10)
            {
                _ownerFsm.ChangeState(new InGameSceneState(_ownerFsm));
            }
        }
        protected override void exitState()
        {
            Debug.Log("B End");
        }
    }
}
