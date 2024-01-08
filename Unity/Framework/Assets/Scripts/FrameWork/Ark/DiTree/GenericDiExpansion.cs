namespace DiTreeGroup
{
    /// <summary>
    /// DiTreeの拡張クラス
    /// </summary>
    public static class GenericDiExpansion
    {
        /// <summary>
        /// DiTreeを生成する
        /// </summary>
        /// <param name="current">この拡張関数を使うインスタンス</param>
        /// <typeparam name="T">この拡張関数を使うインスタンスの型</typeparam>
        /// <typeparam name="TTree">DiTreeのGeneric</typeparam>
        /// <returns>DiTree</returns>
        public static TTree CreateDiTree<T, TTree>(this T current)
            where T : class, IDiField
            where TTree : IDiTree<IDiField>, new()
        {
            return DiTreeInitializer.CreateDiTree<TTree>();
        }

        /// <summary>
        /// DiTreeを生成する
        /// </summary>
        /// <param name="current">この拡張関数を使うインスタンス</param>
        /// <param name="findStartNodeKey">検索を開始するノードのキー</param>
        /// <typeparam name="T">この拡張関数を使うインスタンスの型</typeparam>
        /// <typeparam name="TTree">DiTreeのGeneric</typeparam>
        /// <returns>DiTree</returns>
        public static TTree CreateDiTree<T, TTree>(this T current, in string findStartNodeKey)
            where T : class, IDiField
            where TTree : IDiTree<IDiField>, new()
        {
            return DiTreeInitializer.CreateDiTree<TTree>(findStartNodeKey);
        }
    }
}