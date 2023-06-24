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
        void Init(in string findStartNodeKey);

        /// <summary>
        /// ノードの追加 (外部公開用)
        /// 動的にノードを追加するときに使う
        /// AddNodeとRemoveNodeはセットで使おう
        /// </summary>
        DiNode AddNode(in IDiNodeForInstanceEditing parentNodeEditing, in Type childKey);

        /// <summary>
        /// ノードの追加 (外部公開用)
        /// 動的にノードを追加するときに使う
        /// AddNodeとRemoveNodeはセットで使おう
        /// </summary>
        DiNode AddNodeNotDic(in IDiNodeForInstanceEditing parentNodeEditing, in Type childKey);

        /// <summary>
        /// ノードの削除 (外部公開用)
        /// 動的にノードを削除するときに使う
        /// AddNodeとRemoveNodeはセットで使おう
        /// </summary>
        void RemoveNode(in IDiNodeForInstanceEditing parentNodeEditing, in string key);

        /// <summary>
        /// ノードの削除 (外部公開用)
        /// 動的にノードを削除するときに使う
        /// AddNodeとRemoveNodeはセットで使おう
        /// </summary>
        void RemoveNode(in IDiNodeForInstanceEditing parentNodeEditing, in Type childKey);

        /// <summary>
        /// ノードの削除 (外部公開用)
        /// 動的にノードを削除するときに使う
        /// AddNodeとRemoveNodeはセットで使おう
        /// </summary>
        void RemoveNodeNotDic(in IDiNodeForInstanceEditing parentNodeEditing, in Type childKey);

        /// <summary>
        /// 登録されたインスタンスを取得する
        /// </summary>
        T Get<T>();

        /// <summary>
        /// 登録されたインスタンスを取得する
        /// </summary>
        T Get<T>(in string key);

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
        IDiNodeForInstanceEditing GetNode(in string key);

        /// <summary>
        /// ノードの追加
        /// </summary>
        void AddNode(in DiNode parent, in string key, in DiNode childNode);
    }
}