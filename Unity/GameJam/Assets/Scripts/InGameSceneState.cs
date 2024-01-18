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

        protected override BaseStateBehaviour LoadScenePrefab()
        {
            return Util.LoadScenePrefab<InGameSceneState>(Const.PathInGameScenePrefab, _owner);
        }

        protected override void enterState()
        {
            Util.DebugLog($"{Localize.Get(ETextKind.InGameScene)} Start");
            _goTitleButton.onClick.RemoveAllListeners();
            _goTitleButton.onClick.AddListener(goTitle);

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
        }

        protected override void exitState()
        {
            _testBallPool.Release();

            Util.DebugLog("A End");
            Destroy(gameObject);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                var obj = _testBallPool.Get();
                obj.Init();
                obj.transform.localPosition = Vector3.zero;
            }
            else if(Input.GetKeyDown(KeyCode.W))
            {

            }
        }
    }
}
