namespace SingletonContainer.Example
{
    /// <summary>
    /// シングルトン管理クラスExample
    /// </summary>
    public class ExampleSingletonTree<TOwnerClass> : SingletonTree<TOwnerClass>
        where TOwnerClass : ISingletonField
    {
        protected override void setupTree()
        {
            var nodeA = new SingletonNode();
            var dataA = new ExampleDataA();
            nodeA.RegisterInstance(dataA);

            var nodeB = new SingletonNode();
            var dataB = new ExampleDataB();
            nodeB.RegisterInstance(dataB);

            // ノードを構成
            addNode(s_rootNode, typeof(ExampleClassA), nodeA);
            addNode(s_rootNode, typeof(ExampleClassB), nodeB);
        }
    }
}