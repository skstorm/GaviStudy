using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameJam
{
    public class InGameSceneState : BaseState
    {
        private int count = 0;
        public InGameSceneState(IBaseFsm fsm) : base(fsm)
        {
        }

        protected override void enterState()
        {
            Debug.Log("A Start");
            SceneManager.LoadScene("TitleScene");
        }
        protected override void updateState()
        {
            ++count;
            Debug.Log("A Update" + count);
            if (count > 10)
            {
                _ownerFsm.ChangeState(new TitleSceneState(_ownerFsm));
            }
        }
        protected override void exitState()
        {
            Debug.Log("A End");
        }
    }
}