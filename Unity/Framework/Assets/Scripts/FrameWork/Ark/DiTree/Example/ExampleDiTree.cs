namespace DiTreeGroup.Example
{
    /// <summary>
    /// DiTreeクラスExample
    /// </summary>
    public class ExampleDiTree<TOwnerClass> : DiTree<TOwnerClass>
        where TOwnerClass : IDiField
    {
        /// <summary>
        /// Treeの親子関係を作成
        /// </summary>
        protected override void setupTree()
        {
            var mainNode = new DiNode();
            addNode(SRootNode, typeof(ExampleDiMain), mainNode);
            
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