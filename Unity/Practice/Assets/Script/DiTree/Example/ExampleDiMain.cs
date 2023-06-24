using System.Collections.Generic;

namespace DiTreeGroup.Example
{
    public class ExampleDiMain : ExampleClassBase<ExampleDiMain>
    {
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            // 先にノードを追加してからDiTreeHolderを継承したクラスを作る必要がある
            // DiTreeHolderを継承は生成時、自分がどのノードに属するかを判定するため。その情報がないとエラーが出る
            var currentNode = _tree.GetCurrentNode();
            var logicMgrNode = _tree.AddNode(currentNode, typeof(BattleManagerLogic));
            var viewMgrNode = _tree.AddNode(currentNode, typeof(BattleManagerView));
            
            // ゲームに必要なクラス作成
            // これらはDiTreeHolderから継承されている
            var logicManager = new BattleManagerLogic(); 
            var viewManager = new BattleManagerView();
            
            // DiTreeHolderをリストに格納
            var treeHolderList = new List<DiTreeHolder>();
            treeHolderList.Add(logicManager);
            treeHolderList.Add(viewManager);

            // Treeをセットアップ
            foreach (var treeHolder in treeHolderList)
            {
                treeHolder.SetupTree();
            }
            
            // 各ノードにアクセスしたいInstanceを登録する
            logicMgrNode.RegisterInstance(logicManager);
            viewMgrNode.RegisterInstance(logicManager);

            // 実行
            logicManager.Run();
            viewManager.Run();
        }

        /// <summary>
        /// 実行
        /// </summary>
        public override void Run()
        {
            // とくにやることなし
        }
    }
}