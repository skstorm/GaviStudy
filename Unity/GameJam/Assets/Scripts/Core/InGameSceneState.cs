using UnityEngine;
using UnityEngine.UI;

namespace GameJam
{
    public class InGameSceneState : BaseStateBehaviour
    {
        [SerializeField]
        private Text _text;

        [SerializeField]
        private Button _goTitleButton;

        [SerializeField]
        private TestBallPool _testBallPool;

        private LifeCycle _lifeCycle;

        protected override BaseStateBehaviour LoadScenePrefab()
        {
            return Util.LoadScenePrefab<InGameSceneState>(Const.PathInGameScenePrefab, _owner);
        }

        protected override void enterState()
        {
            Util.DebugLog($"{Localize.Get(ETextKind.InGameScene)} Start");
            _goTitleButton.onClick.RemoveAllListeners();
            _goTitleButton.onClick.AddListener(goTitle);

            SingletonContainer.TestBallQue = new();
            SingletonContainer.TestBallQue.ListInit();

            _lifeCycle = new LifeCycle();
            _lifeCycle.Init(_testBallPool);

            _testBallPool.Init(2);
        }

        private void goTitle()
        {
            var nextState = Util.LoadScenePrefab<TitleSceneState>(Const.PathTitleScenePrefab, _owner);
            _owner.ChangeState(nextState);
        }

        protected override void updateState()
        {
            //Util.DebugLog("A Update" + count);
            _lifeCycle.UpdateLifeCycle();
        }

        protected override void exitState()
        {
            SingletonContainer.TestBallQue.ListRelease();
            _lifeCycle.Release();
            _testBallPool.Release();

            Util.DebugLog("A End");
            Destroy(gameObject);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                SingletonContainer.TestBallQue.Book(1);
                /*
                var obj = _testBallPool.Get();
                obj.Init();
                obj.transform.localPosition = Vector3.zero;
                */
            }
            else if(Input.GetKeyDown(KeyCode.W))
            {

            }
        }
    }
}
