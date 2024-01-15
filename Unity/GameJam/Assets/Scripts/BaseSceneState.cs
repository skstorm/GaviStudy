using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameJam
{
    public class BaseSceneState : MonoBehaviour
    {
        protected IFsm _ownerFsm;

        public void Init(IFsm fsm)
        {
            _ownerFsm = fsm;
        }

        public async UniTask Run()
        {
            enterState();

            do
            {
                await UniTask.DelayFrame(1);
                updateState();

            } while (!_ownerFsm.IsNewState);

            exitState();
        }

        protected virtual void enterState() { }
        protected virtual void updateState() { }
        protected virtual void exitState() { }
    }
}