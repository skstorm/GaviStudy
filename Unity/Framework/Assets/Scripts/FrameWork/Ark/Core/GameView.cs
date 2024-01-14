using Ark.DiTree;
using Ark.Gear;
using Ark.Util;
using DiTreeGroup;
using UnityEngine;

namespace Ark.Core
{
    public interface IGameViewOrder
    {
        IBaseSceneViewOrder SetupSceneView(IBaseSceneLogic sceneLogic);
        IBaseSceneViewOrder StartUpSceneView(IBaseSceneLogic sceneLogic);
    }

    // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
    //! GameViewの基本クラス
    // 必ずCreateSceneViewをオーバーライドする
    abstract public class GameView : ArkDiTreeHolderBehavior<GameView>, IGameViewOrder
    {
        private IBaseSceneView _currentSceneView = null;
        private ILogicStateChnager_ForView _logicStateChanger = null;

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! 初期化関数
        protected override void StartNodeProcess()
        {
            base.StartNodeProcess();
            _logicStateChanger = _tree.Get<LogicStateChanger>();

            ArkLog.Debug("GameView Start");
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! 解除処理
        protected override void EndNodeProcess()
        {
            base.EndNodeProcess();

            ArkLog.Debug("GameView End");
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! 描画処理
        public void Render(int deltaFrame)
        {
            _currentSceneView.Render(deltaFrame);
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! 現在SceneView取得
        public IBaseSceneView GetCurrentSceneView()
        {
            return _currentSceneView;
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! SceneView作成
        abstract protected IBaseSceneView CreateSceneView(IBaseSceneLogic sceneLogic);

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! SceneView設定
        public IBaseSceneViewOrder StartUpSceneView(IBaseSceneLogic sceneLogic)
        {
            // SceneView生成
            _currentSceneView = CreateSceneView(sceneLogic);
            // ギアに追加
            _tree.AddNode(_currentNode, _currentSceneView.GetType());

            return _currentSceneView;
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! SceneView設定
        public IBaseSceneViewOrder SetupSceneView(IBaseSceneLogic sceneLogic)
        {
            // シーンを親から外す
            _currentSceneView.RunAllEndNodeProc();
            _tree.RemoveNode(_currentNode, _currentSceneView.GetType());
            Destroy(_currentSceneView.GameObject);

            // SceneView生成
            _currentSceneView = CreateSceneView(sceneLogic);
            // ギアに追加
            _tree.AddNode(_currentNode, _currentSceneView.GetType());

            return _currentSceneView;
        }
    }
}