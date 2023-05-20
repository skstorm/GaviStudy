using System.Collections;
using System.Collections.Generic;
using HanPractice;
using UnityEngine;

namespace SingletonGroup
{
    public class ExampleMain : BaseExampleClass
    {
        public override void Run()
        {
            SingletonTreeTreeFunc.InitSingletonTree();
            
            var nodeA = new ExampleClassA();
            nodeA.Run();
            
            var nodeB = new ExampleClassB();
            nodeB.Run();
        }
    }
}