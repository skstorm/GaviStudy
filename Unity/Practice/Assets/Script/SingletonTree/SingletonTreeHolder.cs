namespace SingletonContainer
{
    public abstract class SingletonTreeHolder : ISingletonField
    {
        protected ISingletonTree<SingletonTreeHolder> _tree;

        public abstract void Run();
    }
}