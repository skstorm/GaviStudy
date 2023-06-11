namespace SingletonContainer
{
    /// <summary>
    /// 初期化用（指定のタイミングでSingletonTreeを初期化したい場合使う）
    /// </summary>
    public abstract class SingletonTreeTreeInitializer
    {
        /// <summary>
        /// 初期化
        /// シングルトンツリーを使う前に、必ず1回呼ばないといけない
        /// </summary>
        public static void InitSingletonTree<TTree>() where TTree : ISingletonTree<ISingletonField>, new()
        {
            var singletonTree = new TTree();
            singletonTree.OnceInit();
        }

        /// <summary>
        /// シングルトンツリー生成（クラスのフィールドで生成したい時）
        /// </summary>
        public static TTree CreateSingletonTree<TTree>() where TTree : ISingletonTree<ISingletonField>, new()
        {
            var singletonTree = new TTree();
            singletonTree.Init();
            return singletonTree;
        }

        /// <summary>
        /// シングルトンツリー生成（クラスのフィールドで生成したい時）
        /// 検索開始のノードをKeyで指定可能
        /// </summary>
        public static TTree CreateSingletonTree<TTree>(string _findStartNodeKey)  where TTree : ISingletonTree<ISingletonField>, new()
        {
            var singletonTree = new TTree();
            singletonTree.Init(_findStartNodeKey);
            return singletonTree;
        }
    }
}