using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SingletonGroup
{
    public class ExampleBaseClass : ISingletonField
    {
        public virtual void Run()
        {
        }
    }

    public class ExampleClassA : ExampleBaseClass
    {
        public override void Run()
        {
            var tree = this.CreateSingletonTree();
            
            var dataA = tree.Get<ExampleDataA>();
            Debug.Log(dataA.DataA);
        }
    }

    public class ExampleClassB : ExampleBaseClass
    {
        public override void Run()
        {
            var tree = this.CreateSingletonTree();
            
            var dataA = tree.Get<ExampleDataA>();
            Debug.Log(dataA.DataA);
        }
    }
}