using System.Collections.Generic;
using System;

namespace TestSingleton
{
    public class SingletonNode
    {
        private SingletonNode _parent = null;

        private Dictionary<string, SingletonNode> _children = new Dictionary<string, SingletonNode>();

        private Dictionary<string, object> _dicInstance = new Dictionary<string, object>();

        public SingletonNode()
        {
        }

        public void SetParent(SingletonNode parent)
        {
            _parent = parent;
        }

        public void AddChild(string key, SingletonNode child)
        {
            child.SetParent(this);
            _children.Add(key, child);
        }

        public void RemoveChild(string key)
        {
            _children.Remove(key);
        }

        public SingletonNode FindChild(string key)
        {
            // 子供がいなかったらNullを返す
            if (_children.Count <= 0)
            {
                return null;
            }

            // 見つかったら
            if (_children.ContainsKey(key))
            {
                // そいつを返す
                return _children[key];
            }
            else // 見つからなかったら
            {
                // こどもの子供を検索する
                var e = _children.GetEnumerator();
                while (e.MoveNext())
                {
                    return e.Current.Value.FindChild(key);
                }
            }

            return null;
        }

        public void AddInstance(object instance)
        {
            AddInstance(instance.GetType(), instance);
        }

        public void AddInstance(Type clazz, object instance)
        {
            string className = clazz.ToString();
            _dicInstance.Add(className, instance);
        }

        /// 登録されたインスタンスを削除
        public void RemoveInstance(Type clazz)
        {
            string className = clazz.ToString();
            _dicInstance.Remove(className);
        }

        /// クラス名で指定したインスタンスを取得（初期ステップ）
        public T Get<T>()
        {
            Type clazz = typeof(T);
            string className = clazz.ToString();
            return GetWithClassName<T>(className, this);
        }

        /// クラス名で指定したインスタンスを取得（親にさかのぼりながら探索する）
        private T GetWithClassName<T>(string className, SingletonNode node)
        {
            if (_dicInstance.ContainsKey(className))
            {
                /// 自分がそのクラスのインスタンスを保持していれば返す
                return (T)_dicInstance[className];
            }
            else
            {
                /// ないので親に聞いてみる
                if (_parent == null)
                {
                    /// 親なしなので見つからない
                    throw new Exception(string.Format("指定されたクラス{0}は{1}Nodeに登録されていません。;", className, node));
                }

                return _parent.GetWithClassName<T>(className, node);
            }
        }
    }

    public abstract class SingletonTree<TNodeKey, TOwnerClass>
    {
        protected static SingletonNode _root = new SingletonNode();

        private static Dictionary<string, SingletonNode> _dicNode = new Dictionary<string, SingletonNode>();

        private static bool _isFirst = true;

        protected SingletonNode _currentNode = null;

        private const string ROOT_KEY = "Root";

        public SingletonTree()
        {
            if (_isFirst)
            {
                _dicNode.Add(ROOT_KEY, _root);
                InitNode();
                _isFirst = false;
            }

            string currentKey = typeof(TOwnerClass).ToString();
            _currentNode = _root.FindChild(currentKey);
        }

        protected abstract void InitNode();

        protected void AddNode(SingletonNode parent, TNodeKey childKey, SingletonNode childNode)
        {
            _dicNode.Add(childKey.ToString(), childNode);
            parent.AddChild(childKey.ToString(), childNode);
        }

        public T Get<T>()
        {
            return _currentNode.Get<T>();
        }

        public static void Clear()
        {
            _root = null;
            _dicNode = null;
        }
    }

    public class TempDataA
    {
        public string A = "A";
    }

    public class TempDataA1
    {
        public string A1 = "A1";
    }

    public class TempDataAA
    {
        public string AA = "AA";
    }

    public class TempDataB
    {
        public string B = "B";
    }

    public class TestTreeA
    {
    }

    public class TestTreeAA
    {
    }

    public class TestTreeB
    {
    }

    public class TestSingletonTree<TOwnerClass> : SingletonTree<Type, TOwnerClass>
    {
        public TestSingletonTree() : base()
        {
        }

        protected override void InitNode()
        {
            TempDataA tempDataA = new TempDataA();
            TempDataA1 tempDataA1 = new TempDataA1();
            TempDataB tempDataB = new TempDataB();

            SingletonNode a = new SingletonNode();
            a.AddInstance(tempDataA);
            a.AddInstance(tempDataA1);

            SingletonNode b = new SingletonNode();
            b.AddInstance(tempDataB);

            SingletonNode aa = new SingletonNode();

            AddNode(_root, typeof(TestTreeA), a);
            AddNode(_root, typeof(TestTreeB), b);

            AddNode(a, typeof(TestTreeAA), aa);
        }
    }
}