using UnityEngine;

namespace DiTreeGroup.Example
{
    public class ExampleDiBehaviour : MonoBehaviour
    {
        private void OnEnable()
        {
            DiTreeInitializer.InitDiTree<ExampleDiTree<IDiField>>();
            
            var main = new ExampleDiMain();
            main.Run();
        }
    }
}