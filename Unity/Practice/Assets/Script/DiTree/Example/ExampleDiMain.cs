using System.Collections.Generic;

namespace DiTreeGroup.Example
{
    public class ExampleDiMain : ExampleClassBase<ExampleDiMain>
    {
        public void Init()
        {
            // 先にノードを追加してからInstanceを作る必要がある
            var currentNode = _tree.GetCurrentNode();
            var logicMgrNode = _tree.AddNode(currentNode, typeof(BattleManagerLogic));
            var viewMgrNode = _tree.AddNode(currentNode, typeof(BattleManagerView));
            
            var treeHolderList = new List<DiTreeHolder>();
            var logicManager = new BattleManagerLogic(); 
            var viewManager = new BattleManagerView();
            treeHolderList.Add(logicManager);
            treeHolderList.Add(viewManager);

            foreach (var treeHolder in treeHolderList)
            {
                treeHolder.SetupTree();
            }
            
            logicMgrNode.RegisterInstance(logicManager);
            viewMgrNode.RegisterInstance(logicManager);

            logicManager.Run();
            viewManager.Run();
        }

        public override void Run()
        {
        }
    }
}