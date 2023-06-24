using System.Collections.Generic;
using HanPractice;

namespace DiTreeGroup.Example
{
    /// <summary>
    /// Exampleメイン
    /// </summary>
    public class ExampleMain : BaseExampleClass
    {
        /// <summary>
        /// 実行
        /// </summary>
        public override void Run()
        {
            // DiTree初期化
            DiTreeInitializer.InitDiTree<ExampleDiTree<IDiField>>();

            // ゲームに必要なクラス作成
            // これらはDiTreeHolderから継承されている
            var systemManager = new BattleManagerLogic(); 
            var sceneManager = new BattleManagerView();
            
            // リストに格納
            var treeHolderList = new List<DiTreeHolder>();
            treeHolderList.Add(systemManager);
            treeHolderList.Add(sceneManager);

            // Treeセットアップ
            foreach (var treeHolder in treeHolderList)
            {
                treeHolder.SetupTree();
            }

            // 実行
            systemManager.Run();
            sceneManager.Run();
        }
    }
}