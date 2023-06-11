using System;

namespace SingletonContainer
{
    public interface IDiForInstanceEditing
    {
        /// <summary>インスタンス追加</summary>
        void RegisterInstance(object instance);
        /// <summary>インスタンス追加</summary>
        void RegisterInstance(Type type, object instance);
        /// <summary>インスタンス追加</summary>
        void RegisterInstance(string className, object instance);

        /// <summary>登録されたインスタンスを削除</summary>
        void UnregisterInstance(object instance);
        /// <summary>登録されたインスタンスを削除</summary>
        void UnregisterInstance(Type type);
        /// <summary>登録されたインスタンスを削除</summary>
        void UnregisterInstance(string key);

        /// <summary>登録された全てのインスタンスを削除</summary>
        void AllUnregisterInstance();
    }   
}