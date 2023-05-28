using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SingletonGroup.Example
{
    public class ExampleBaseClass : ISingletonField
    {
        protected ISingletonTree<ExampleBaseClass> _tree;

        public virtual void Run()
        {
        }
    }

    public class ExampleClassA : ExampleBaseClass
    {
        public override void Run()
        {
            var tree = this.CreateSingletonTree();
            _tree = tree;

            var dataA = _tree.Get<ExampleDataA>();
            Debug.Log(dataA.DataA);
        }
    }

    public class ExampleClassB : ExampleBaseClass
    {
        public override void Run()
        {
            var tree = this.CreateSingletonTree();
            
            var data = tree.Get<ExampleDataB>();
            Debug.Log(data.DataB);
        }
    }
}