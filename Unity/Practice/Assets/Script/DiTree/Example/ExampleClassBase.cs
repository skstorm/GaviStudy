using System.Collections.Generic;
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
}