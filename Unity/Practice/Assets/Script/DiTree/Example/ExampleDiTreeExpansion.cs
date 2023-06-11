namespace DiTreeGroup.Example
{
    public static class ExampleDiTreeExpansion
    {
        /// <summary>
        /// DiTreeを生成する
        /// </summary>
        public static ExampleDiTree<T> CreateDiTree<T>(this T current)
            where T : class, IDiField
        {
            return current.CreateDiTree<T, ExampleDiTree<T>>();
        }
    }
}