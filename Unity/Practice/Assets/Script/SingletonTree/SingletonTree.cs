using System;
using System.Collections.Generic;

namespace SingletonGroup
{
    /// <summary>
    /// SingletonTreeの基底クラス（Static変数宣言用）
    /// </summary>
    public class BaseSingletonTree
    {
        /// <summary> ルートのノードキー </summary>
        protected const string ROOT_KEY = "Root";

        /// <summary> 最上位ノード </summary>
        protected static SingletonNode s_rootNode = null;

        /// <summary> ノードの辞書 </summary>
        protected static Dictionary<string, SingletonNode> s_dicNode = null;
        
        /// <summary>
        /// 解除処理
        /// </summary>
        public static void Clear()
        {
            s_rootNode = null;
            s_dicNode = null;
        }
    }

    /// <summary>
    /// SingletonTreeの基底クラス（通常変数や、機能実装用）
    /// </summary>
    public abstract class SingletonTree<TOwnerClass> : BaseSingletonTree, ISingletonTree<TOwnerClass> 
        where TOwnerClass : ISingletonField
    {
        /// <summary> 現在ノード（このノードから親へと検索していく） </summary>
        protected SingletonNode _currentNode = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected SingletonTree()
        {
        }

        /// <summary>
        /// 最初一回だけ行われる初期化処理
        /// </summary>
        public void OnceInit()
        {
            s_rootNode = new SingletonNode();
            s_dicNode = new Dictionary<string, SingletonNode>();
            // 辞書にルートを入れとく
            s_dicNode.Add(ROOT_KEY, s_rootNode);
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
            _currentNode = s_rootNode.FindChild(currentKey);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Init(string findStartNodeKey)
        {
            // 現在ノードを取得
            string currentKey = typeof(TOwnerClass).ToString();
            SingletonNode node = s_dicNode[findStartNodeKey];
            _currentNode = node.FindChild(currentKey);
            if (_currentNode == null)
            {
                _currentNode = node;
            }
        }

        /// <summary>
        /// ツリーを構築する
        /// </summary>
        protected abstract void setupTree();

        /// <summary>
        /// ノードの追加
        /// </summary>
        protected void addNode(SingletonNode parentNode, Type childKey, SingletonNode childNode)
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
        public SingletonNode AddNode(ISingletonNodeForInstanceEditing parentNodeEditing, Type childKey)
        {
            var childNode = new SingletonNode();
            var parentNode = (SingletonNode)parentNodeEditing;
            addNode(parentNode, childKey, childNode);
            return childNode;
        }

        /// <summary>
        /// ノードの追加 (外部公開用)
        /// 動的にノードを追加するときに使う
        /// AddNodeとRemoveNodeはセットで使おう
        /// </summary>
        public SingletonNode AddNodeNotDic(ISingletonNodeForInstanceEditing parentNodeEditing, Type childKey)
        {
            var childNode = new SingletonNode();
            var parentNode = (SingletonNode)parentNodeEditing;
            var key = childKey.ToString();
            parentNode.AddChild(key, childNode);
            return childNode;
        }

        /// <summary>
        /// ノードの削除 (外部公開用)
        /// 動的にノードを削除するときに使う
        /// AddNodeとRemoveNodeはセットで使おう
        /// </summary>
        public void RemoveNode(ISingletonNodeForInstanceEditing parentNodeEditing, string key)
        {
            s_dicNode.Remove(key);
            var parentNode = (SingletonNode)parentNodeEditing;
            parentNode.RemoveChild(key);
        }

        /// <summary>
        /// ノードの削除 (外部公開用)
        /// 動的にノードを削除するときに使う
        /// AddNodeとRemoveNodeはセットで使おう
        /// </summary>
        public void RemoveNode(ISingletonNodeForInstanceEditing parentNodeEditing, Type childKey)
        {
            var key = childKey.ToString();
            RemoveNode(parentNodeEditing, key);
        }

        /// <summary>
        /// ノードの削除 (外部公開用)
        /// 動的にノードを削除するときに使う
        /// AddNodeとRemoveNodeはセットで使おう
        /// </summary>
        public void RemoveNodeNotDic(ISingletonNodeForInstanceEditing parentNodeEditing, Type childKey)
        {
            var key = childKey.ToString();
            var parentNode = (SingletonNode)parentNodeEditing;
            parentNode.RemoveChild(key);
        }

        /// <summary>
        /// 登録されたインスタンスを取得する
        /// </summary>
        public T Get<T>()
        {
            return _currentNode.Get<T>();
        }

        /// <summary>
        /// 登録されたインスタンスを取得する
        /// </summary>
        public T Get<T>(string key)
        {
            return _currentNode.Get<T>(key);
        }

        /// <summary>
        /// 現在ノード取得（インスタンス編集用）
        /// </summary>
        public ISingletonNodeForInstanceEditing GetCurrentNodeForInstanceEditing()
        {
            return _currentNode;
        }

        /// <summary>
        /// 現在ノード取得
        /// </summary>
        public SingletonNode GetCurrentNode()
        {
            return _currentNode;
        }

        /// <summary>
        /// ノード取得
        /// </summary>
        public ISingletonNodeForInstanceEditing GetNode(string key)
        {
            return s_rootNode.FindChild(key);
        }

        /// <summary>
        /// ノードの追加
        /// </summary>
        public void AddNode(SingletonNode parent, string key, SingletonNode childNode)
        {
            s_dicNode.Add(key, childNode);
            parent.AddChild(key, childNode);
        }

        /*
         * 継承先で以下の関数を実装する必要がある。
         * XXXXSingletonTree はSingletonTreeを継承したクラス
         * 
        /// <summary>
        /// 生成関数
        /// </summary>
        public static XXXXSingletonTree<TOwnerClass> Create()
        {
            return new XXXXSingletonTree<TOwnerClass>();
        }
        */
    }
}