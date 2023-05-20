using HanPractice;
using UnityEngine;

namespace TestSingleton
{
    public class TestSingletonManager : BaseExampleClass
    {
        public override void Run()
        {
            {
                TestSingletonTree<TestTreeA> tree = new TestSingletonTree<TestTreeA>();

                {
                    TempDataA tempDataA = tree.Get<TempDataA>();
                    Debug.Log(tempDataA.A);
                }

                {
                    TempDataA1 tempDataA1 = tree.Get<TempDataA1>();
                    Debug.Log(tempDataA1.A1);
                }
            }

            {
                TestSingletonTree<TestTreeAA> tree = new TestSingletonTree<TestTreeAA>();
                {
                    TempDataA tempDataA = tree.Get<TempDataA>();
                    Debug.Log(tempDataA.A);
                }

                {
                    TempDataA1 tempDataA1 = tree.Get<TempDataA1>();
                    Debug.Log(tempDataA1.A1);
                }
            }

            {
                TestSingletonTree<TestTreeB> tree = new TestSingletonTree<TestTreeB>();
                {
                    var tempDataA = tree.Get<TempDataB>();
                    Debug.Log(tempDataA.B);
                }
            }
        }
    }
}