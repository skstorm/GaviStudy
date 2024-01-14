using UnityEngine;
using Ark.Gear;
using Ark.Util;
using DiTreeGroup;
using DiTreeGroup.Example;
using System.Collections.Generic;
using Ark.DiTree;

namespace Ark.Core
{
    public class GameLoop : ArkDiTreeHolder<GameLoop>
    {
        private FrameManager _frameManager = null;
        private GameLogic _gameLogic = null;
        private GameView _gameView = null;
        private LogicStateChanger _logicStateChanger = null;
        private CommandRecorder _commandRecorder = null;
        private CommandReplayer _commandReplayer = null;
        private DataLoadManager _dataLoadManager = null;

        private float _debugLastUpdateSeconds = 0;
        private float _debugDeltaFrame = 0;
        private int _debugCount = 0;

        public GameLoop(ISetting setting, GameView gameView, DataLoadManager dataLoadManager) : base(true)
        {
            _frameManager = new FrameManager(setting.Fps);
            _gameLogic = new GameLogic(setting);
            _gameView = gameView;
            _gameView.InitDi();
            _logicStateChanger = new LogicStateChanger();
            _commandRecorder = new CommandRecorder();
            _commandReplayer = new CommandReplayer();
            _dataLoadManager = dataLoadManager;

            var currentNode = _tree.GetCurrentNode();
            
            // DiTreeHolderをリストに格納
            var treeHolderList = new List<IDiTreeHolder>();
            //treeHolderList.Add(_frameManager);
            treeHolderList.Add(_gameLogic);
            treeHolderList.Add(_gameView);
            //treeHolderList.Add(_logicStateChanger);
            //treeHolderList.Add(_commandRecorder);
            //treeHolderList.Add(_commandReplayer);

            // Treeをセットアップ
            foreach (var treeHolder in treeHolderList)
            {
                treeHolder.SetupTree();
            }
			

            // 各ノードにアクセスしたいInstanceを登録する
            currentNode.RegisterInstance(_frameManager);
            currentNode.RegisterInstance(_gameLogic);
            currentNode.RegisterInstance(_gameView);
            currentNode.RegisterInstance(_logicStateChanger);
            currentNode.RegisterInstance(_commandRecorder);
            currentNode.RegisterInstance(_commandReplayer);

            //_gameLogic.Run();
        }

        public override void Run()
        {
        }


        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! 初期化
        protected void StartGearProcess()
        {
            ArkLog.Debug("Game Loop Start");

            // FrameManagerの時間初期化
            _frameManager.RecordLastUpdateSeconds();
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! 解除
        protected void EndGearProcess()
        {
            ArkLog.Debug("Game Loop End");
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! 更新
        public void Update()
        {
            int deltaFrame = _frameManager.CalcDeltaFrame();
            _frameManager.RecordLastUpdateSeconds();

            //////////////////// debug ////////////////
            _debugCount += deltaFrame;
            _debugDeltaFrame += (Time.time - _debugLastUpdateSeconds);
            if (_debugDeltaFrame >= 1.0f)
            {
                //ArkLog.Debug("count : "+ _debugCount);
                _debugDeltaFrame = 0.0f;
                _debugCount = 0;
            }
            _debugLastUpdateSeconds = Time.time;
            //////////////////// debug ////////////////

            for (int i = 0; i < deltaFrame; ++i)
            {
                _gameLogic.Update();
            }

            if (deltaFrame > 0)
            {
                _gameView.Render(deltaFrame);
            }
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! 現在SceneView取得
        public IBaseSceneView GetCurrentSceneView()
        {
            return _gameView.GetCurrentSceneView();
        }
    }
}