namespace DiTreeGroup
{
    /// <summary>
    /// DiTreeを持っているクラス
    /// </summary>
    public abstract class DiTreeHolder : IDiField
    {
        /// <summary>DiTreeのInterface</summary>
        protected readonly IDiTree<IDiField> _tree;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="tree">DiTreeのInterface</param>
        protected DiTreeHolder(in IDiTree<IDiField> tree)
        {
            _tree = tree;
        }

        /// <summary>
        /// Treeセットアップ
        /// </summary>
        public virtual void SetupTree()
        {

        }
    }
}