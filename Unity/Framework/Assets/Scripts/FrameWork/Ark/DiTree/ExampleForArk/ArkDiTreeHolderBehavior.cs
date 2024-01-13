using DiTreeGroup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ark.DiTree
{

    public abstract class ArkDiTreeHolderBehavior<T> : DiTreeHolderBehavior, IDiField
        where T : class, IDiField
    {
        public override void InitDi()
        {
            InitDi(DiTreeInitializer.CreateDiTree<ArkDiTree<T>>(true));
        }
    }
}