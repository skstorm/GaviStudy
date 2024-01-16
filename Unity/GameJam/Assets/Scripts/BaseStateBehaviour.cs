using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameJam
{
    public abstract class BaseStateBehaviour : MonoBehaviour, IBaseState
    {
        protected IStateMachine _owner;

        private static bool _staticInit = false;

        private void Awake()
        {
            if (_staticInit)
            {
                return;
            }
            _staticInit = true;

            var gameStateMachine = new StateMachine();
            Init(gameStateMachine);

            var startState = LoadScenePrefab();
            DontDestroyOnLoad(startState.gameObject);

            var origin = Resources.Load<MainGameObject>("Prefabs/MainGameObject");
            var mainGameObj = GameObject.Instantiate<MainGameObject>(origin);
            mainGameObj.Run(gameStateMachine, startState).Forget();

            SceneManager.LoadScene("BootScene");
        }

        public void Init(IStateMachine stateMachine)
        {
            _owner = stateMachine;
        }

        public async UniTask Run()
        {
            enterState();

            do
            {
                await UniTask.DelayFrame(1);
                updateState();

            } while (!_owner.IsNewState);

            exitState();
        }

        protected abstract BaseStateBehaviour LoadScenePrefab();

        protected virtual void enterState() { }
        protected virtual void updateState() { }
        protected virtual void exitState() { }

        public virtual void Release() 
        {
            Destroy(this);
        }
    }
}