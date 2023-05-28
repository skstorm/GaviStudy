using HanPractice;

namespace SingletonContainer.Example
{
    public class ExampleMain : BaseExampleClass
    {
        public override void Run()
        {
            SingletonTreeTreeInitializer.InitSingletonTree<ExampleSingletonTree<ISingletonField>>();
            
            var nodeA = new ExampleClassA();
            nodeA.Run();
            
            var nodeB = new ExampleClassB();
            nodeB.Run();
        }
    }
}