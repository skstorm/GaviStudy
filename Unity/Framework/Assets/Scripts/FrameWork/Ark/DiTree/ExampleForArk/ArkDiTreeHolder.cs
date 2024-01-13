using DiTreeGroup;
using Ark.DiTree;
using System;

namespace Ark.DiTree
{
    public abstract class ArkDiTreeHolder<T> : DiTreeHolder
        where T : class, IDiField
    {

        protected ArkDiTreeHolder() : base(DiTreeInitializer.CreateDiTree<ArkDiTree<T>>())
        {
        }

        public abstract void Run();
    }

}