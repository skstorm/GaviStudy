using System;
using System.Collections.Generic;

namespace SingletonGroup
{
    public class SingletonNode : ISingletonNodeForInstanceEditing
    {
        /// <summary> 親 </summary>
        private SingletonNode _parent = null;

        /// <summary> 子供 </summary>
        private Dictionary<string, SingletonNode> _dicChild = new();

        /// <summary> 保持するインスタンスの辞書 </summary>
        private Dictionary<string, object> _dicInstance = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SingletonNode()
        {
        }

        /// <summary>
        /// 親ノードを設定
        /// </summary>
        public void SetParent(SingletonNode parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// 子供ノードを追加する
        /// </summary>
        public void AddChild(string key, SingletonNode child)
        {
            child.SetParent(this);
            _dicChild.Add(key, child);
        }

        /// <summary>
        /// 子供ノードを削除する
        /// </summary>
        public void RemoveChild(string key)
        {
            _dicChild.Remove(key);
        }

        /// <summary>
        /// 子供ノードからKeyのノードを検索する
        /// </summary>
        public SingletonNode FindChild(string key)
        {
            // 子供がいなかったらNullを返す
            if (_dicChild.Count <= 0)
            {
                return null;
            }

            // 見つかったら
            if (_dicChild.ContainsKey(key))
            {
                // そいつを返す
                return _dicChild[key];
            }
            else // 見つからなかったら
            {
                // こどもの子供を検索する
                var e = _dicChild.GetEnumerator();

                while (e.MoveNext())
                {
                    SingletonNode node = e.Current.Value.FindChild(key);

                    // nullでない子供が見つかったらそいつを返す
                    if (node != null)
                    {
                        return node;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// インスタンス追加
        /// </summary>
        public void AddInstance(object instance)
        {
            AddInstance(instance.GetType(), instance);
        }

        /// <summary>
        /// インスタンス追加
        /// </summary>
        public void AddInstance(Type type, object instance)
        {
            string className = type.ToString();
            AddInstance(className, instance);
        }

        /// <summary>
        /// インスタンス追加
        /// </summary>
        public void AddInstance(string className, object instance)
        {
            _dicInstance.Add(className, instance);
        }

        /// <summary>
        /// 登録されたインスタンスを削除
        /// </summary>
        public void RemoveInstance(object instance)
        {
            RemoveInstance(instance.GetType());
        }

        /// <summary>
        /// 登録されたインスタンスを削除
        /// </summary>
        public void RemoveInstance(Type type)
        {
            string className = type.ToString();
            RemoveInstance(className);
        }

        /// <summary>
        /// 登録されたインスタンスを削除
        /// </summary>
        public void RemoveInstance(string key)
        {
            _dicInstance.Remove(key);
        }

        /// <summary>
        /// 登録された全てのインスタンスを削除
        /// </summary>
        public void AllRemoveInstance()
        {
            _dicInstance.Clear();
        }

        /// <summary>
        /// クラス名で指定したインスタンスを取得（初期ステップ）
        /// </summary>
        public T Get<T>()
        {
            Type clazz = typeof(T);
            string className = clazz.ToString();
            return getWithClassName<T>(className, this);
        }

        /// <summary>
        /// クラス名で指定したインスタンスを取得（初期ステップ）
        /// </summary>
        public T Get<T>(string key)
        {
            return getWithClassName<T>(key, this);
        }

        /// <summary>
        /// クラス名で指定したインスタンスを取得（親にさかのぼりながら探索する）
        /// </summary>
        private T getWithClassName<T>(string className, SingletonNode node)
        {
            if (_dicInstance.ContainsKey(className))
            {
                // 自分がそのクラスのインスタンスを保持していれば返す
                return (T)_dicInstance[className];
            }
            else
            {
                // ないので親に聞いてみる
                if (_parent == null)
                {
                    // 親なしなので見つからない
                    //return default(T);
                    throw new Exception(string.Format("指定されたクラス{0}は{1}Nodeに登録されていません。;", className, node));
                }

                return _parent.getWithClassName<T>(className, node);
            }
        }
    }
}