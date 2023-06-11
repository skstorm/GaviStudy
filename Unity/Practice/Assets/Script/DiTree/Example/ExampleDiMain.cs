using System.Collections.Generic;

namespace DiTreeGroup.Example
{
    public class ExampleDiMain : ExampleClassBase<ExampleDiMain>
    {
        public override void Run()
        {
            var treeHolderList = new List<DiTreeHolder>();
            var systemManager = new BattleSystemManager(); 
            var sceneManager = new BattleSceneManager();
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