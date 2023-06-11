namespace DiTreeGroup
{
    /// <summary>
    /// SingletonTreeを持っているクラス
    /// </summary>
    public abstract class DiTreeHolder : IDiField
    {
        /// <summary>SingletonTreeのInterface</summary>
        protected readonly IDiTree<IDiField> _tree;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="tree">SingletonTreeのInterface</param>
        protected DiTreeHolder(IDiTree<IDiField> tree)
        {
            _tree = tree;
        }
    }
}