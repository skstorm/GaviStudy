using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class BaseStateBehaviour : MonoBehaviour, IBaseState
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

        public virtual void Release() 
        {
            Destroy(this);
        }
    }
}