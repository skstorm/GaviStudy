using System;

namespace DiTreeGroup
{
    public interface IDiNodeForInstanceEditing
    {
        /// <summary>インスタンス追加</summary>
        void RegisterInstance(in object instance);
        /// <summary>インスタンス追加</summary>
        void RegisterInstance(in Type type, in object instance);
        /// <summary>インスタンス追加</summary>
        void RegisterInstance(in string className, in object instance);

        /// <summary>登録されたインスタンスを削除</summary>
        void UnregisterInstance(in object instance);
        /// <summary>登録されたインスタンスを削除</summary>
        void UnregisterInstance(in Type type);
        /// <summary>登録されたインスタンスを削除</summary>
        void UnregisterInstance(in string key);

        /// <summary>登録された全てのインスタンスを削除</summary>
        void AllUnregisterInstance();
    }   
}