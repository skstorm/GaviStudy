namespace SingletonGroup
{
    /// <summary>
    /// ユグドラシル
    /// シングルトン管理クラス
    /// </summary>
    public class Yggdrasil<TOwnerClass> : SingletonTree<TOwnerClass>
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
                // FkeFieldLogic
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