namespace SingletonGroup
{
    /// <summary>
    /// 初期化用（指定のタイミングでSingletonTreeを初期化したい場合使う）
    /// </summary>
    public class SingletonTreeTreeFunc : ISingletonField
    {
        /// <summary>
        /// 初期化
        /// シングルトンツリーを使う前に、必ず1回呼ばないといけない
        /// </summary>
        public static void InitSingletonTree()
        {
            var singletonTree = Yggdrasil<SingletonTreeTreeFunc>.Create();
            singletonTree.OnceInit();
        }

        /// <summary>
        /// シングルトンツリー生成（クラスのフィールドで生成したい時）
        /// ＊＊注意＊＊
        /// １．通常メソッドの中では使わないこと
        /// ２．TNodeTypeはこの関数を呼ぶクラスの型にすること
        /// </summary>
        public static Yggdrasil<TNodeType> CreateSingletonTree<TNodeType>() where TNodeType : class, ISingletonField
        {
            var singletonTree = Yggdrasil<TNodeType>.Create();
            singletonTree.Init();
            return singletonTree;
        }

        /// <summary>
        /// シングルトンツリー生成（クラスのフィールドで生成したい時）
        /// 検索開始のノードをKeyで指定可能
        /// ＊＊注意＊＊
        /// １．通常メソッドの中では使わないこと
        /// ２．TNodeTypeはこの関数を呼ぶクラスの型にすること
        /// </summary>
        public static Yggdrasil<TNodeType> CreateSingletonTree<TNodeType>(string _findStartNodeKey) where TNodeType : class, ISingletonField
        {
            var singletonTree = Yggdrasil<TNodeType>.Create();
            singletonTree.Init(_findStartNodeKey);
            return singletonTree;
        }
    }

    /// <summary>
    /// SingletonTreeの拡張クラス
    /// </summary>
    public static class GenericExpansionSingletonTree
    {
        /// <summary>
        /// SingletonTreeを生成する
        /// </summary>
        public static Yggdrasil<T> CreateSingletonTree<T>(this T _current) where T : class, ISingletonField
        {
            return SingletonTreeTreeFunc.CreateSingletonTree<T>();
        }

        /// <summary>
        /// SingletonTreeを生成する
        /// </summary>
        public static Yggdrasil<T> CreateSingletonTree<T>(this T _current, string _findStartNodeKey) where T : class, ISingletonField
        {
            return SingletonTreeTreeFunc.CreateSingletonTree<T>(_findStartNodeKey);
        }
    }

    /// <summary>
    /// ユグドラシル
    /// シングルトン管理クラス
    /// </summary>
    public class Yggdrasil<TOwnerClass> : SingletonTree<TOwnerClass> where TOwnerClass : ISingletonField
    {
        protected Yggdrasil() : base()
        {

        }

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
                // ノードを構成
                addNode(s_rootNode, typeof(ExampleClassA), nodeA);
                addNode(s_rootNode, typeof(ExampleClassB), nodeB);
            }
        }

        /// <summary>
        /// 生成関数
        /// </summary>
        public static Yggdrasil<TOwnerClass> Create()
        {
            return new Yggdrasil<TOwnerClass>();
        }
    }
}
