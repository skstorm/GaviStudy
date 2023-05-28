using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SingletonContainer
{
    /// <summary>
    /// 初期化用（指定のタイミングでSingletonTreeを初期化したい場合使う）
    /// SingletonTreeTreeInitializer
    /// </summary>
    public abstract class SingletonTreeTreeInitializer
    {
        /// <summary>
        /// 初期化
        /// シングルトンツリーを使う前に、必ず1回呼ばないといけない
        /// </summary>
        public static void InitSingletonTree<TTree>() where TTree : SingletonTree<ISingletonField>, new()
        {
            //var singletonTree = Yggdrasil<ISingletonField>.Create();
            var singletonTree = new TTree();
            singletonTree.OnceInit();
        }

        /// <summary>
        /// シングルトンツリー生成（クラスのフィールドで生成したい時）
        /// ＊＊注意＊＊
        /// １．通常メソッドの中では使わないこと
        /// ２．TNodeTypeはこの関数を呼ぶクラスの型にすること
        /// </summary>
        public static TTree CreateSingletonTree<TTree>() where TTree : ISingletonTree<ISingletonField>, new()
        {
            var singletonTree = new TTree();
            singletonTree.Init();
            return singletonTree;
        }

        /// <summary>
        /// シングルトンツリー生成（クラスのフィールドで生成したい時）
        /// 検索開始のノードをKeyで指定可能
        /// ＊＊注意＊＊
        /// １．通常メソッドの中では使わないこと
        /// ２．TNodeTypeはこの関数を呼ぶクラスの型にすること
        /// </summary>
        public static TTree CreateSingletonTree<TTree>(string _findStartNodeKey)  where TTree : ISingletonTree<ISingletonField>, new()
        {
            var singletonTree = new TTree();
            singletonTree.Init(_findStartNodeKey);
            return singletonTree;
        }
    }
}