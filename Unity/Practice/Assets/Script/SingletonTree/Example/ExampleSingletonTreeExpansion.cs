namespace SingletonContainer.Example
{
    public static class ExampleSingletonTreeExpansion
    {
        /// <summary>
        /// SingletonTreeを生成する
        /// </summary>
        public static ExampleSingletonTree<T> CreateSingletonTree<T>(this T current)
            where T : class, ISingletonField
        {
            return current.CreateSingletonTree<T, ExampleSingletonTree<T>>();
        }
    }
}