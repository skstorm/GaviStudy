namespace DiTreeGroup
{
    /// <summary>
    /// DiTreeを持っているクラス
    /// </summary>
    public abstract class DiTreeHolder : IDiField, IDiTreeHolder
    {
        /// <summary>DiTreeのInterface</summary>
        protected readonly IDiTree<IDiField> _tree;

        protected DiNode _currentNode;

        /// <summary>
        /// 現在ノード取得
        /// </summary>
        public DiNode GetCurrentNode() => _currentNode;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="tree">DiTreeのInterface</param>
        protected DiTreeHolder(in IDiTree<IDiField> tree)
        {
            _tree = tree;
            if(_tree.IsInit)
            {
                _currentNode = _tree.GetCurrentNode();
                _currentNode.SetStartNodeAction(StartNodeProcess);
                _currentNode.SetEndNodeAction(EndNodeProcess);
            }
        }

        public void InitDiTree()
        {
            _tree.Init();
            _currentNode = _tree.GetCurrentNode();
            _currentNode.SetStartNodeAction(StartNodeProcess);
            _currentNode.SetEndNodeAction(EndNodeProcess);
        }

        /// <summary>
        /// Treeセットアップ
        /// </summary>
        public virtual void SetupTree()
        {
        }

        protected virtual void StartNodeProcess()
        {
        }

        protected virtual void EndNodeProcess()
        {
        }

        public void RunAllStartNodeProc()
        {
            _currentNode.RunAllStartNodeProc();
        }

        public void RunAllEndNodeProc()
        {
            _currentNode.RunAllEndNodeProc();
        }
    }
}