namespace SingletonContainer
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
    public static class XXXXSingletonTreeExpansion
    {
        /// <summary>
        /// SingletonTreeを生成する
        /// </summary>
        public static XXXXSingletonTree<T> CreateSingletonTree<T>(this T _current)
            where T : class, ISingletonField
        {
            return _current.CreateSingletonTree<T, XXXXSingletonTree<T>>();
        }
    }
    */
}