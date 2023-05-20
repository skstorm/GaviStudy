using System;

namespace SingletonGroup
{
    public interface ISingletonNodeForInstanceEditing
    {
        /// <summary>インスタンス追加</summary>
        void AddInstance(object instance);
        /// <summary>インスタンス追加</summary>
        void AddInstance(Type type, object instance);
        /// <summary>インスタンス追加</summary>
        void AddInstance(string className, object instance);

        /// <summary>登録されたインスタンスを削除</summary>
        void RemoveInstance(object instance);
        /// <summary>登録されたインスタンスを削除</summary>
        void RemoveInstance(Type type);
        /// <summary>登録されたインスタンスを削除</summary>
        void RemoveInstance(string key);

        /// <summary>登録された全てのインスタンスを削除</summary>
        void AllRemoveInstance();
    }   
}