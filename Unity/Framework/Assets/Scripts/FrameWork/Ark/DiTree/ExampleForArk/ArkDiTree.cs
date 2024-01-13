using Ark.Core;
using DiTreeGroup;
using DiTreeGroup.Example;
using Example;
using UnityEngine;

namespace Ark.DiTree
{
    public class ArkDiTree<TOwnerClass> : DiTree<TOwnerClass>
        where TOwnerClass : IDiField
    {
        /// <summary>
        /// Treeの親子関係を作成
        /// </summary>
        protected override void setupTree()
        {
            var mainNode = AddNode(SRootNode, typeof(GameLoop));
            var frameMgrNode = AddNode(mainNode, typeof(FrameManager));
            var gameLogicNode = AddNode(mainNode, typeof(GameLogic));
            var gameViewNode = AddNode(mainNode, typeof(GameView));
            var logicStateChangerNode = AddNode(mainNode, typeof(LogicStateChanger));
            var cmdRecorderNode = AddNode(mainNode, typeof(CommandRecorder));
            var cmdReplayerNode = AddNode(mainNode, typeof(CommandReplayer));

            var menuSceneLogicNode = AddNode(gameLogicNode, typeof(MenuSceneLogic));
            var menuSceneViewNode = AddNode(gameViewNode, typeof(MenuSceneView));


            /*
             ↓のようにあらかじめ全ノードの構成を作っておく方法も可能
            var nodeA = new DiNode();
            var dataA = new ExampleDataA();
            nodeA.RegisterInstance(dataA);

            var nodeB = new DiNode();
            var dataB = new ExampleDataB();
            nodeB.RegisterInstance(dataB);

            // ノードを構成
            addNode(SRootNode, typeof(BattleSystemManager), nodeA);
            addNode(SRootNode, typeof(BattleSceneManager), nodeB);
*/
        }
    }
}