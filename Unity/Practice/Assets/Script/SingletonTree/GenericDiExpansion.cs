namespace SingletonContainer
{
    /// <summary>
    /// SingletonTreeの拡張クラス
    /// </summary>
    public static class GenericSingletonTreeExpansion
    {
        /// <summary>
        /// SingletonTreeを生成する
        /// </summary>
        /// <param name="current">この拡張関数を使うインスタンス</param>
        /// <typeparam name="T">この拡張関数を使うインスタンスの型</typeparam>
        /// <typeparam name="TTree">SingletonTreeのGeneric</typeparam>
        /// <returns>SingletonTree</returns>
        public static TTree CreateSingletonTree<T, TTree>(this T current)
            where T : class, IDiField
            where TTree : IDiTree<IDiField>, new()
        {
            return DiTreeTreeInitializer.CreateDiTree<TTree>();
        }

        /// <summary>
        /// SingletonTreeを生成する
        /// </summary>
        /// <param name="current">この拡張関数を使うインスタンス</param>
        /// <param name="findStartNodeKey">検索を開始するノードのキー</param>
        /// <typeparam name="T">この拡張関数を使うインスタンスの型</typeparam>
        /// <typeparam name="TTree">SingletonTreeのGeneric</typeparam>
        /// <returns>SingletonTree</returns>
        public static TTree CreateSingletonTree<T, TTree>(this T current, string findStartNodeKey)
            where T : class, IDiField
            where TTree : IDiTree<IDiField>, new()
        {
            return DiTreeTreeInitializer.CreateDiTree<TTree>(findStartNodeKey);
        }
    }
}