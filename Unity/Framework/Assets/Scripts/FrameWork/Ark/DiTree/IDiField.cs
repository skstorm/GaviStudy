namespace DiTreeGroup
{
    /// <summary>
    /// DiTreeを取得可能にするInterface
    /// </summary>
    public interface IDiField
    {
    }
    /*
     * 以下のクラスはDiTreeを継承したクラスで実装する必要がある
     * XXXXDiTree はDiTreeを継承したクラス
     * 
    public static class XXXXDiTreeExpansion
    {
        /// <summary>
        /// SingletonTreeを生成する
        /// </summary>
        public static XXXXDiTree<T> CreateDiTree<T>(this T _current)
            where T : class, ISingletonField
        {
            return _current.CreateDiTree<T, XXXXDiTree<T>>();
        }
    }
    */
}