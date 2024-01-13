using Ark.Gear;
using DiTreeGroup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiTreeGroup
{
    public interface IDiTreeHolderBehavior : IDiTreeHolder
    {
        void InitDi(bool isDiTreeInit);
    }

    public abstract class DiTreeHolderBehavior : MonoBehaviour, IDiTreeHolderBehavior
    {
        protected IDiTree<IDiField> _tree;

        protected DiNode _currentNode;

        public DiNode GetCurrentNode() => _currentNode;

        public abstract void InitDi(bool isDiTreeInit);


        protected void InitDi(in IDiTree<IDiField> tree)
        {
            _tree = tree;
            _currentNode = _tree.GetCurrentNode();
            _currentNode.SetStartNodeAction(StartNodeProcess);
            _currentNode.SetEndNodeAction(EndNodeProcess);
        }

        public void InitDiTree()
        {
            InitDi(true);
        }

        /// <summary>
        /// Tree«»«Ã«È«¢«Ã«×
        /// </summary>
        public virtual void SetupTree()
        {
        }

        protected virtual void StartNodeProcess()
        {
        }

        protected virtual void EndNodeProcess()
        {
        }

        public void RunAllStartNodeProc()
        {
            _currentNode.RunAllStartNodeProc();
        }

        public void RunAllEndNodeProc()
        {
            _currentNode.RunAllEndNodeProc();
        }
    }
}