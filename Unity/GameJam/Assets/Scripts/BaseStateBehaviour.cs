using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class BaseStateBehaviour : MonoBehaviour, IBaseState
    {
        protected IBaseFsm _ownerFsm;

        public static T Load<T>(string path, IBaseFsm fsm) where T : BaseStateBehaviour
        {
            var origin = Resources.Load<T>(path);
            var obj = Instantiate<T>(origin);
            obj.Init(fsm);
            return obj;
        }

        public void Init(IBaseFsm fsm)
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