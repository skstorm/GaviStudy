using UnityEngine;
using System;

namespace HanPractice
{
    public class EtcMain : MonoBehaviour
    {
        [SerializeField]
        private ExampleInfo[] exampleInfos = null;

        // Start is called before the first frame update
        void OnEnable()
        {
            for(int i=0; i< exampleInfos.Length; ++i)
            {
                var info = exampleInfos[i];
                if(info.IsRun)
                {
                    info.Example.Run();
                }
            }
        }
    }

    [Serializable]
    public class ExampleInfo
    {
        [SerializeField]
        private string name = string.Empty;

        [SerializeField]
        private bool isRun = true;
        public bool IsRun { get { return isRun; } }

        [SerializeField]
        private BaseExampleClass example = null;
        public BaseExampleClass Example { get { return example; } }
    }

    [Serializable]
    public class BaseExampleClass: MonoBehaviour
    {
        public virtual void Run() { }
    }
}