namespace DiTreeGroup
{
    /// <summary>
    /// 初期化用（指定のタイミングでSingletonTreeを初期化したい場合使う）
    /// </summary>
    public abstract class DiTreeInitializer
    {
        /// <summary>
        /// 初期化
        /// Diツリーを使う前に、必ず1回呼ばないといけない
        /// </summary>
        public static void InitDiTree<TTree>() where TTree : IDiTree<IDiField>, new()
        {
            var diTree = new TTree();
            diTree.OnceInit();
        }

        /// <summary>
        /// Diツリー生成（クラスのフィールドで生成したい時）
        /// </summary>
        public static TTree CreateDiTree<TTree>(bool isDiTreeInit) where TTree : IDiTree<IDiField>, new()
        {
            var diTree = new TTree();
            if(isDiTreeInit)
            {
                diTree.Init();
            }
            return diTree;
        }

        /// <summary>
        /// Diツリー生成（クラスのフィールドで生成したい時）
        /// 検索開始のノードをKeyで指定可能
        /// </summary>
        public static TTree CreateDiTree<TTree>(in string _findStartNodeKey)  where TTree : IDiTree<IDiField>, new()
        {
            var diTree = new TTree();
            diTree.Init(_findStartNodeKey);
            return diTree;
        }
    }
}