using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SingletonContainer
{
    public interface ISingletonTree<out TOwnerClass> where TOwnerClass : ISingletonField
    {
        /// <summary>
        /// 最初一回だけ行われる初期化処理
        /// </summary>
        void OnceInit();

        /// <summary>
        /// 初期化
        /// </summary>
        void Init();

        /// <summary>
        /// 初期化
        /// </summary>
        void Init(string findStartNodeKey);

        /// <summary>
        /// ノードの追加 (外部公開用)
        /// 動的にノードを追加するときに使う
        /// AddNodeとRemoveNodeはセットで使おう
        /// </summary>
        SingletonNode AddNode(ISingletonNodeForInstanceEditing parentNodeEditing, Type childKey);

        /// <summary>
        /// ノードの追加 (外部公開用)
        /// 動的にノードを追加するときに使う
        /// AddNodeとRemoveNodeはセットで使おう
        /// </summary>
        SingletonNode AddNodeNotDic(ISingletonNodeForInstanceEditing parentNodeEditing, Type childKey);

        /// <summary>
        /// ノードの削除 (外部公開用)
        /// 動的にノードを削除するときに使う
        /// AddNodeとRemoveNodeはセットで使おう
        /// </summary>
        void RemoveNode(ISingletonNodeForInstanceEditing parentNodeEditing, string key);

        /// <summary>
        /// ノードの削除 (外部公開用)
        /// 動的にノードを削除するときに使う
        /// AddNodeとRemoveNodeはセットで使おう
        /// </summary>
        void RemoveNode(ISingletonNodeForInstanceEditing parentNodeEditing, Type childKey);

        /// <summary>
        /// ノードの削除 (外部公開用)
        /// 動的にノードを削除するときに使う
        /// AddNodeとRemoveNodeはセットで使おう
        /// </summary>
        void RemoveNodeNotDic(ISingletonNodeForInstanceEditing parentNodeEditing, Type childKey);

        /// <summary>
        /// 登録されたインスタンスを取得する
        /// </summary>
        T Get<T>();

        /// <summary>
        /// 登録されたインスタンスを取得する
        /// </summary>
        T Get<T>(string key);

        /// <summary>
        /// 現在ノード取得（インスタンス編集用）
        /// </summary>
        ISingletonNodeForInstanceEditing GetCurrentNodeForInstanceEditing();

        /// <summary>
        /// 現在ノード取得
        /// </summary>
        SingletonNode GetCurrentNode();

        /// <summary>
        /// ノード取得
        /// </summary>
        ISingletonNodeForInstanceEditing GetNode(string key);

        /// <summary>
        /// ノードの追加
        /// </summary>
        void AddNode(SingletonNode parent, string key, SingletonNode childNode);
    }
}