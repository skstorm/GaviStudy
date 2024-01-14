using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiTreeGroup
{
    public interface IDiTreeHolder
    {
        /// <summary>
        /// 現在ノード取得
        /// </summary>
        DiNode GetCurrentNode();

        Type GetType();

        /// <summary>
        /// Treeセットアップ
        /// </summary>
        void SetupTree();

        void InitDi();

        void RunAllStartNodeProc();

        void RunAllEndNodeProc();
    }
}