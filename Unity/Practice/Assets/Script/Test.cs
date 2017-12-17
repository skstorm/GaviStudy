using UnityEngine;
using System.Collections;

public class TestClassA
{
	public int a = 0;
}

public class Test : MonoBehaviour
{

	// Use this for initialization
	void Start () {
		TestClassA ca = null;
		Func(ref ca);
		ca.a = 2;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	private void Func(ref TestClassA ca)
	{
		ca = new TestClassA();
		ca.a = 1;
	}
}
