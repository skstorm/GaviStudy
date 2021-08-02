namespace Fsm
{
    /// <summary>
    /// SingtonのInterface
    /// </summary>
    public interface ISingleton
    {
        /// <summary>  </summary>
        void Init();
        /// <summary>  </summary>
        void Release();
    }

    /// <summary>
    /// Singleton
    /// </summary>
    public abstract class Singleton<T> where T : ISingleton, new()
    {
        /// <summary> Instance </summary>
        protected static T s_instance;

        /// <summary> コンストラクタ </summary>
        protected Singleton()
        {
        }

        /// <summary> 初期化 </summary>
        public abstract void Init();
        /// <summary> 開放 </summary>
        public abstract void Release();

        /// <summary> Instance生成 </summary>
        public static T CreateInstance()
        {
            s_instance = new T();
            s_instance.Init();
            return s_instance;
        }

        /// <summary> Instance取得 </summary>
        public static T GetInstance()
        {
            return s_instance;
        }

        /// <summary> Instance開放 </summary>
        public static void ReleaseInstance()
        {
            s_instance.Release();
            s_instance = default(T);
        }
    }
}