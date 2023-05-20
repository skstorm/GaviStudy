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
        public static Yggdrasil<T> CreateSingletonTree<T>(this T _current)
            where T : class, ISingletonField
        {
            return SingletonTreeTreeInitializer.CreateSingletonTree<Yggdrasil<T>>();
        }

        /// <summary>
        /// SingletonTreeを生成する
        /// </summary>
        public static Yggdrasil<T> CreateSingletonTree<T>(this T _current, string _findStartNodeKey)
            where T : class, ISingletonField
        {
            return SingletonTreeTreeInitializer.CreateSingletonTree<Yggdrasil<T>>(_findStartNodeKey);
        }
    }
}