using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSingletonManager : MonoBehaviour 
{
	void Start () 
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
				TempDataA tempDataA = tree.Get<TempDataA>();
				Debug.Log(tempDataA.A);
			}

			{
				TempDataA1 tempDataA1 = tree.Get<TempDataA1>();
				Debug.Log(tempDataA1.A1);
			}
		}
	}
}