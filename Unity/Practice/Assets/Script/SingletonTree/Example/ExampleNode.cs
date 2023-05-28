using UnityEngine;

namespace SingletonContainer.Example
{
    public abstract class ExampleClassBase<T> : SingletonTreeHolder
        where T : class, ISingletonField
    {
        protected ExampleClassBase() : base(SingletonTreeTreeInitializer.CreateSingletonTree<ExampleSingletonTree<T>>())
        {
        }
        
        public abstract void Run();
    }

    public class ExampleClassA : ExampleClassBase<ExampleClassA>
    {
        public override void Run()
        {
            var dataA = _tree.Get<ExampleDataA>();
            Debug.Log(dataA.DataA);
        }
    }

    public class ExampleClassB : ExampleClassBase<ExampleClassB>
    {
        public override void Run()
        {
            var tree = this.CreateSingletonTree();
            var data = tree.Get<ExampleDataB>();
            Debug.Log(data.DataB);
        }
    }
}