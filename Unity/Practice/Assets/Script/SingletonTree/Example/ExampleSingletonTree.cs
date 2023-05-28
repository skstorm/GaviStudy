namespace SingletonGroup.Example
{
    /// <summary>
    /// シングルトン管理クラスExample
    /// </summary>
    public class ExampleSingletonTree<TOwnerClass> : SingletonTree<TOwnerClass>
        where TOwnerClass : ISingletonField
    {
        protected override void setupTree()
        {
            // クウカ飛ぶ
            {
                // 
                var nodeA = new SingletonNode();
                var dataA = new ExampleDataA();
                nodeA.AddInstance(dataA);
                // 
                var nodeB = new SingletonNode();
                var dataB = new ExampleDataB();
                nodeB.AddInstance(dataB);
                // ノードを構成
                addNode(s_rootNode, typeof(ExampleClassA), nodeA);
                addNode(s_rootNode, typeof(ExampleClassB), nodeB);
            }
        }
    }
}