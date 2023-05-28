namespace SingletonContainer
{
    /// <summary>
    /// SingletonTreeを持っているクラス
    /// </summary>
    public abstract class SingletonTreeHolder : ISingletonField
    {
        /// <summary>SingletonTreeのInterface</summary>
        protected readonly ISingletonTree<ISingletonField> _tree;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="tree">SingletonTreeのInterface</param>
        protected SingletonTreeHolder(ISingletonTree<ISingletonField> tree)
        {
            _tree = tree;
        }
    }
}