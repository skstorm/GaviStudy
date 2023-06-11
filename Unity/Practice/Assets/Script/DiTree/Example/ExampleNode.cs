using UnityEngine;

namespace DiTreeGroup.Example
{
    public abstract class ExampleClassBase<T> : DiTreeHolder
        where T : class, IDiField
    {
        protected ExampleClassBase() : base(DiTreeInitializer.CreateDiTree<ExampleDiTree<T>>())
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
            var tree = this.CreateDiTree();
            var data = tree.Get<ExampleDataB>();
            Debug.Log(data.DataB);
        }
    }
}