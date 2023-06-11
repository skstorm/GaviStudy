using UnityEngine;

namespace DiTreeGroup.Example
{
    public class TestExampleMain : MonoBehaviour
    {
        private void OnEnable()
        {
            DiTreeInitializer.InitDiTree<ExampleDiTree<IDiField>>();
            
            var nodeA = new ExampleClassA();
            nodeA.Run();

            var nodeB = new ExampleClassB();
            nodeB.Run();
        }
    }
}