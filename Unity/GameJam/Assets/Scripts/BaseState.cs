using Cysharp.Threading.Tasks;

namespace GameJam
{
    public interface IBaseState
    {
        UniTask Run();
        void Release();
    }


    public abstract class BaseState : IBaseState
    {
        protected IFsm _ownerFsm;

        public BaseState(IFsm fsm)
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

        public virtual void Release() { }
    }
}