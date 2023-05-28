namespace SingletonGroup
{
    /// <summary>
    /// SingletonTreeの拡張クラス
    /// </summary>
    public static class GenericSingletonTreeExpansion
    {
        /// <summary>
        /// SingletonTreeを生成する
        /// </summary>
        public static TTree CreateSingletonTree<T, TTree>(this T _current)
            where T : class, ISingletonField
            where TTree : ISingletonTree<ISingletonField>, new()
        {
            return SingletonTreeTreeInitializer.CreateSingletonTree<TTree>();
        }

        /// <summary>
        /// SingletonTreeを生成する
        /// </summary>
        public static TTree CreateSingletonTree<T, TTree>(this T _current, string _findStartNodeKey)
            where T : class, ISingletonField
            where TTree : ISingletonTree<ISingletonField>, new()
        {
            return SingletonTreeTreeInitializer.CreateSingletonTree<TTree>(_findStartNodeKey);
        }
    }
}