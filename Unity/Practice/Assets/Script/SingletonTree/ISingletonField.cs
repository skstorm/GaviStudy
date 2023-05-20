namespace SingletonGroup
{
    /// <summary>
    /// SingletonTreeを取得可能にするInterface
    /// </summary>
    public interface ISingletonField
    {
    }
    /*
     * 以下のクラスはSingletonTreeを継承したクラスで実装する必要がある
     * XXXXSingletonTree はSingletonTreeを継承したクラス
     * 
    /// <summary>
    /// 初期化用（指定のタイミングでSingletonTreeを初期化したい場合使う）
    /// </summary>
    public class SingletonTreeTreeInitializer : ISingletonField
    {
        public static void InitSingletonTree()
        {
            var singletonTree = XXXXSingletonTree<SingletonTreeTreeInitialzer>.Create();
            singletonTree.OnceInit();
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
        public static XXXXSingletonTree<T> CreateSingletonTree<T>(this T current) where T : ISingletonField
        {
            var singletonTree = XXXXSingletonTree<T>.Create();
            singletonTree.Init();
            return singletonTree;
        }
    }
    */
}