using HanPractice;

namespace DiTreeGroup.Example
{
    public class ExampleMain : BaseExampleClass
    {
        public override void Run()
        {
            DiTreeInitializer.InitDiTree<ExampleDiTree<IDiField>>();
            
            var nodeA = new ExampleClassA();
            nodeA.Run();

            var nodeB = new ExampleClassB();
            nodeB.Run();
        }
    }
}