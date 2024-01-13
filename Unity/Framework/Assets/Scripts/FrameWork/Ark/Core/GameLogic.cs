using Ark.DiTree;
using Ark.Gear;
using Ark.Util;
using Example;

namespace Ark.Core
{
    public interface IGameLogic_ForLogicStateChanger
    {
        void NotifyCommand(ICommand command);
    }

    public interface IGameLogic_ForSceneLogic
    {
        void ChangeScene(IBaseSceneLogic nextScene);
    }

    public class GameLogic : ArkDiTreeHolder<GameLogic>, IGameLogic_ForLogicStateChanger, IGameLogic_ForSceneLogic
    {
        // GameView
        private IGameViewOrder _gameView = null;
        // 現在シーンのLogic
        private IBaseSceneLogic _currentSceneLogic = null;
        // 前のシーンLogic
        private IBaseSceneLogic _prevSceneLogic = null;

        public GameLogic(ISetting setting) : base(true)
        {
            // 開始シーン設定
            _currentSceneLogic = setting.CreateStartScene();
        }

        public override void Run()
        {
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! 初期化
        protected override void StartNodeProcess()
        {
            base.StartNodeProcess();

            _gameView = _tree.Get<MyGameView>();

            _tree.AddNode(_currentNode, _currentSceneLogic.GetType());
            _currentSceneLogic.InitDiTree();

            IBaseSceneViewOrder sceneView = _gameView.StartUpSceneView(_currentSceneLogic);
            _currentSceneLogic.SetSceneViewOrder(sceneView);

            sceneView.InitDi(true);

            ArkLog.Debug("Game Logic Start");
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! 解除
        protected override void EndNodeProcess()
        {
            base.EndNodeProcess();
            ArkLog.Debug("Game Logic End");
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! 更新
        public void Update()
        {
            _currentSceneLogic.Update();
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! シーン変更
        public void ChangeScene(IBaseSceneLogic nextScene)
        {
            // 現在のSceneLogicが抜ける時の処理
            _currentSceneLogic.Exit();
            // 現在のSceneLogicを前のSceneLogicに格納する
            _prevSceneLogic = _currentSceneLogic;
            // ギアの親子関係から外す
            _prevSceneLogic.RunAllEndNodeProc();
            _tree.RemoveNode(_currentNode, _prevSceneLogic.GetType());
            // 現在のSceneLogicを新しいものに入れ替える
            _currentSceneLogic = nextScene;
            // SceneView作成
            IBaseSceneViewOrder sceneView = _gameView.SetupSceneView(_currentSceneLogic);
            // SceneViewを設定
            _currentSceneLogic.SetSceneViewOrder(sceneView);
            // 新しいSceneLogicを子供として追加
            _tree.AddNode(_currentNode, _currentSceneLogic.GetType());
            _currentSceneLogic.InitDiTree();
            _currentSceneLogic.RunAllStartNodeProc();

            sceneView.InitDi(true);
            sceneView.RunAllStartNodeProc();

            // 現在のSceneLogicに入る時の処理
            _currentSceneLogic.Enter();
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! コマンド通達
        public void NotifyCommand(ICommand command)
        {
            _currentSceneLogic.CommandProcess(command);
        }
    }
}