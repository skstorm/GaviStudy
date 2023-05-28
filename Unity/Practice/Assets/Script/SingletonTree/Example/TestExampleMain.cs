using UnityEngine;

namespace SingletonContainer.Example
{
    public class TestExampleMain : MonoBehaviour
    {
        private void OnEnable()
        {
            SingletonTreeTreeInitializer.InitSingletonTree<ExampleSingletonTree<ISingletonField>>();
            
            var nodeA = new ExampleClassA();
            nodeA.Run();

            var nodeB = new ExampleClassB();
            nodeB.Run();
        }
    }
}