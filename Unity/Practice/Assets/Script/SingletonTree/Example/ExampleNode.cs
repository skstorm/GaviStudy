using UnityEngine;

namespace SingletonContainer.Example
{
    public class ExampleClassA : SingletonTreeHolder
    {
        public override void Run()
        {
            var tree = this.CreateSingletonTree();
            _tree = tree;

            var dataA = _tree.Get<ExampleDataA>();
            
            Debug.Log(dataA.DataA);
        }
    }

    public class ExampleClassB : SingletonTreeHolder
    {
        public override void Run()
        {
            var tree = this.CreateSingletonTree();
            
            var data = tree.Get<ExampleDataB>();
            Debug.Log(data.DataB);
        }
    }
}