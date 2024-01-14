using UnityEngine;

namespace DiTreeGroup.Example
{
    /// <summary>
    /// ExampleのEntryポイント
    /// </summary>
    public class ExampleDiBehaviour : MonoBehaviour
    {
        private void OnEnable()
        {
            // DiTree初期化
            DiTreeInitializer.InitDiTree<ExampleDiTree<IDiField>>();
            
            // Example作成
            var main = new ExampleDiMain();
            main.Init();
        }
    }
}