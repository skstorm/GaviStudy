namespace SingletonContainer
{
    public abstract class SingletonTreeHolder : ISingletonField
    {
        protected readonly ISingletonTree<ISingletonField> _tree;

        protected SingletonTreeHolder(ISingletonTree<ISingletonField> tree)
        {
            _tree = tree;
        }
    }
}