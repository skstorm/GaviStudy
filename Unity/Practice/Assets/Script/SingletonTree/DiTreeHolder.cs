namespace SingletonContainer
{
    /// <summary>
    /// SingletonTreeを持っているクラス
    /// </summary>
    public abstract class SingletonTreeHolder : ISingletonField
    {
        /// <summary>SingletonTreeのInterface</summary>
        protected readonly IDiTree<ISingletonField> _tree;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="tree">SingletonTreeのInterface</param>
        protected SingletonTreeHolder(IDiTree<ISingletonField> tree)
        {
            _tree = tree;
        }
    }
}