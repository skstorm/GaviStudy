namespace SingletonGroup.Example
{
    public static class ExampleSingletonTreeExpansion
    {
        /// <summary>
        /// SingletonTreeを生成する
        /// </summary>
        public static ExampleSingletonTree<T> CreateSingletonTree<T>(this T _current)
            where T : class, ISingletonField
        {
            return _current.CreateSingletonTree<T, ExampleSingletonTree<T>>();
        }
    }
}