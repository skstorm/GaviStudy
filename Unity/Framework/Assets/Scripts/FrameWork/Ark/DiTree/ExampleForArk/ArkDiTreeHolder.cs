using DiTreeGroup;
using Ark.DiTree;
using System;

namespace Ark.DiTree
{
    public abstract class ArkDiTreeHolder<T> : DiTreeHolder
        where T : class, IDiField
    {

        protected ArkDiTreeHolder(bool isDiTreeInit) : base(DiTreeInitializer.CreateDiTree<ArkDiTree<T>>(isDiTreeInit))
        {
        }

        public abstract void Run();
    }

}