using UnityEngine;
using System.Collections;

public class Singleton<T> where T : class, new()
{
	protected static T s_instance;

	protected Singleton()
	{
	}

	public static T GetInstance()
	{
		if (s_instance == null)
		{
			s_instance = new T();
		}
		return s_instance;
	}
}
