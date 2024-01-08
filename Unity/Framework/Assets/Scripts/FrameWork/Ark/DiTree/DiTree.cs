using System;
using System.Collections.Generic;

namespace DiTreeGroup
{
    /// <summary>
    /// DiTreeの基底クラス（Static変数宣言用）
    /// </summary>
    public class BaseDiTree
    {
        /// <summary> ルートのノードキー </summary>
        protected const string ROOT_KEY = "Root";

        /// <summary> 最上位ノード </summary>
        protected static DiNode SRootNode = null;

        /// <summary> ノードの辞書 </summary>
        protected static Dictionary<string, DiNode> s_dicNode = null;
        
        /// <summary>
        /// 解除処理
        /// </summary>
        public static void Clear()
        {
            SRootNode = null;
            s_dicNode = null;
        }
    }

    /// <summary>
    /// DiTreeの基底クラス（通常変数や、機能実装用）
    /// </summary>
    public abstract class DiTree<TOwnerClass> : BaseDiTree, IDiTree<TOwnerClass> 
        where TOwnerClass : IDiField
    {
        /// <summary> 現在ノード（このノードから親へと検索していく） </summary>
        protected DiNode CurrentNode = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected DiTree()
        {
        }

        /// <summary>
        /// 最初一回だけ行われる初期化処理
        /// </summary>
        public void OnceInit()
        {
            SRootNode = new DiNode();
            s_dicNode = new Dictionary<string, DiNode>();
            // 辞書にルートを入れとく
            s_dicNode.Add(ROOT_KEY, SRootNode);
            // ツリー構築
            setupTree();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init()
        {
            // 現在ノードを取得
            string currentKey = typeof(TOwnerClass).ToString();
            CurrentNode = SRootNode.FindChild(currentKey);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init(in string findStartNodeKey)
        {
            // 現在ノードを取得
            string currentKey = typeof(TOwnerClass).ToString();
            DiNode node = s_dicNode[findStartNodeKey];
            CurrentNode = node.FindChild(currentKey);
            if (CurrentNode == null)
            {
                CurrentNode = node;
            }
        }

        /// <summary>
        /// ツリーを構築する
        /// </summary>
        protected abstract void setupTree();

        /// <summary>
        /// ノードの追加
        /// </summary>
        protected void addNode(in DiNode parentNode, in Type childKey, in DiNode childNode)
        {
            string key = childKey.ToString();
            s_dicNode.Add(key, childNode);
            parentNode.AddChild(key, childNode);
        }

        /// <summary>
        /// ノードの追加 (外部公開用)
        /// 動的にノードを追加するときに使う
        /// AddNodeとRemoveNodeはセットで使おう
        /// </summary>
        public DiNode AddNode(in IDiNodeForInstanceEditing parentNodeEditing, in Type childKey)
        {
            var childNode = new DiNode();
            var parentNode = (DiNode)parentNodeEditing;
            addNode(parentNode, childKey, childNode);
            return childNode;
        }

        /// <summary>
        /// ノードの追加 (外部公開用)
        /// 動的にノードを追加するときに使う
        /// AddNodeとRemoveNodeはセットで使おう
        /// </summary>
        public DiNode AddNodeNotDic(in IDiNodeForInstanceEditing parentNodeEditing, in Type childKey)
        {
            var childNode = new DiNode();
            var parentNode = (DiNode)parentNodeEditing;
            var key = childKey.ToString();
            parentNode.AddChild(key, childNode);
            return childNode;
        }

        /// <summary>
        /// ノードの削除 (外部公開用)
        /// 動的にノードを削除するときに使う
        /// AddNodeとRemoveNodeはセットで使おう
        /// </summary>
        public void RemoveNode(in IDiNodeForInstanceEditing parentNodeEditing, in string key)
        {
            s_dicNode.Remove(key);
            var parentNode = (DiNode)parentNodeEditing;
            parentNode.RemoveChild(key);
        }

        /// <summary>
        /// ノードの削除 (外部公開用)
        /// 動的にノードを削除するときに使う
        /// AddNodeとRemoveNodeはセットで使おう
        /// </summary>
        public void RemoveNode(in IDiNodeForInstanceEditing parentNodeEditing, in Type childKey)
        {
            var key = childKey.ToString();
            RemoveNode(parentNodeEditing, key);
        }

        /// <summary>
        /// ノードの削除 (外部公開用)
        /// 動的にノードを削除するときに使う
        /// AddNodeとRemoveNodeはセットで使おう
        /// </summary>
        public void RemoveNodeNotDic(in IDiNodeForInstanceEditing parentNodeEditing, in Type childKey)
        {
            var key = childKey.ToString();
            var parentNode = (DiNode)parentNodeEditing;
            parentNode.RemoveChild(key);
        }

        /// <summary>
        /// 登録されたインスタンスを取得する
        /// </summary>
        public T Get<T>()
        {
            return CurrentNode.Get<T>();
        }

        /// <summary>
        /// 登録されたインスタンスを取得する
        /// </summary>
        public T Get<T>(in string key)
        {
            return CurrentNode.Get<T>(key);
        }

        /// <summary>
        /// 現在ノード取得（インスタンス編集用）
        /// </summary>
        public IDiNodeForInstanceEditing GetCurrentNodeForInstanceEditing()
        {
            return CurrentNode;
        }

        /// <summary>
        /// 現在ノード取得
        /// </summary>
        public DiNode GetCurrentNode()
        {
            return CurrentNode;
        }

        /// <summary>
        /// ノード取得
        /// </summary>
        public IDiNodeForInstanceEditing GetNode(in string key)
        {
            return SRootNode.FindChild(key);
        }

        /// <summary>
        /// ノードの追加
        /// </summary>
        public void AddNode(in DiNode parent, in string key, in DiNode childNode)
        {
            s_dicNode.Add(key, childNode);
            parent.AddChild(key, childNode);
        }

        /*
         * 継承先で以下の関数を実装する必要がある。
         * XXXXDiTree はDiTreeを継承したクラス
         * 
        /// <summary>
        /// 生成関数
        /// </summary>
        public static XXXXDiTree<TOwnerClass> Create()
        {
            return new XXXXDiTree<TOwnerClass>();
        }
        */
    }
}