using System.Collections.Generic;

namespace DiTreeGroup.Example
{
    public class ExampleDiMain : ExampleClassBase<ExampleDiMain>
    {
        public void Init()
        {
            // 先にノードを追加してからInstanceを作る必要がある
            var currentNode = _tree.GetCurrentNode();
            var sysMgrNode = _tree.AddNode(currentNode, typeof(BattleSystemManager));
            var sceneMgrNode = _tree.AddNode(currentNode, typeof(BattleSceneManager));
            
            var treeHolderList = new List<DiTreeHolder>();
            var systemManager = new BattleSystemManager(); 
            var sceneManager = new BattleSceneManager();
            treeHolderList.Add(systemManager);
            treeHolderList.Add(sceneManager);

            foreach (var treeHolder in treeHolderList)
            {
                treeHolder.SetupTree();
            }
            
            sysMgrNode.RegisterInstance(systemManager);
            sceneMgrNode.RegisterInstance(systemManager);

            systemManager.Run();
            sceneManager.Run();
        }

        public override void Run()
        {
        }
    }
}