using System;

namespace DiTreeGroup
{
    public interface IDiTree<out TOwnerClass> where TOwnerClass : IDiField
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
        DiNode AddNode(IDiNodeForInstanceEditing parentNodeEditing, Type childKey);

        /// <summary>
        /// ノードの追加 (外部公開用)
        /// 動的にノードを追加するときに使う
        /// AddNodeとRemoveNodeはセットで使おう
        /// </summary>
        DiNode AddNodeNotDic(IDiNodeForInstanceEditing parentNodeEditing, Type childKey);

        /// <summary>
        /// ノードの削除 (外部公開用)
        /// 動的にノードを削除するときに使う
        /// AddNodeとRemoveNodeはセットで使おう
        /// </summary>
        void RemoveNode(IDiNodeForInstanceEditing parentNodeEditing, string key);

        /// <summary>
        /// ノードの削除 (外部公開用)
        /// 動的にノードを削除するときに使う
        /// AddNodeとRemoveNodeはセットで使おう
        /// </summary>
        void RemoveNode(IDiNodeForInstanceEditing parentNodeEditing, Type childKey);

        /// <summary>
        /// ノードの削除 (外部公開用)
        /// 動的にノードを削除するときに使う
        /// AddNodeとRemoveNodeはセットで使おう
        /// </summary>
        void RemoveNodeNotDic(IDiNodeForInstanceEditing parentNodeEditing, Type childKey);

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
        IDiNodeForInstanceEditing GetCurrentNodeForInstanceEditing();

        /// <summary>
        /// 現在ノード取得
        /// </summary>
        DiNode GetCurrentNode();

        /// <summary>
        /// ノード取得
        /// </summary>
        IDiNodeForInstanceEditing GetNode(string key);

        /// <summary>
        /// ノードの追加
        /// </summary>
        void AddNode(DiNode parent, string key, DiNode childNode);
    }
}