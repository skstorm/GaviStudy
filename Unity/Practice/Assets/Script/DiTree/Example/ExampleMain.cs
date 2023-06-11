using System.Collections.Generic;
using HanPractice;

namespace DiTreeGroup.Example
{
    public class ExampleMain : BaseExampleClass
    {
        public override void Run()
        {
            DiTreeInitializer.InitDiTree<ExampleDiTree<IDiField>>();

            var treeHolderList = new List<DiTreeHolder>();
            var systemManager = new BattleManagerLogic(); 
            var sceneManager = new BattleManagerView();
            treeHolderList.Add(systemManager);
            treeHolderList.Add(sceneManager);

            foreach (var treeHolder in treeHolderList)
            {
                treeHolder.SetupTree();
            }

            systemManager.Run();
            sceneManager.Run();
        }
    }
}