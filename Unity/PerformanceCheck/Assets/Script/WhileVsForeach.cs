using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhileVsForeach : MonoBehaviour
{
	
	// Use this for initialization
	void Start () 
	{
		Dictionary<int, string> dic = new Dictionary<int, string>();

		for(int i=0; i<1000; ++i)
		{
			dic.Add(i, string.Empty);
		}

		PerformanceChecker dicPerfChecker = new PerformanceChecker("foreach vs while", 1);

		dicPerfChecker.CheckAction("foreach", (i) =>
		{
			foreach(var v in dic)
			{

			}
		});

		dicPerfChecker.CheckAction("while", (i) =>
		{
			var v = dic.GetEnumerator();
			while(v.MoveNext())
			{

			}
		});

		dicPerfChecker.ShowResult();
	}
}
