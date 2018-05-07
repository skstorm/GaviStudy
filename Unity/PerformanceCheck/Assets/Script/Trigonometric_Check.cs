using UnityEngine;
using System;

public class Trigonometric_Check : MonoBehaviour 
{
	public class AimingData
	{
		public Vector3[] PosArray = null;
		public float[] LengthArray = null;
		public Vector3[] AimDirArray = null;

		public AimingData(int count, float length)
		{
			PosArray = new Vector3[count];
			LengthArray = new float[count];
			for (int i = 0; i < count; ++i)
			{
				LengthArray[i] = length;
			}
			AimDirArray = new Vector3[count];
		}
	}

	void CalcAim(AimingData aimingData, Vector3 target, int count)
	{
		for(int i=0; i<count; ++i)
		{
			aimingData.AimDirArray[i] = Vector3.Normalize(target - aimingData.PosArray[i]) * aimingData.LengthArray[i];
		}
	}

	public class TestObj
	{
		public Vector3 Pos;
		public Vector3 AimDir;
		public float Length = 10;

		public TestObj()
		{

		}

		public void Aim(Vector3 target)
		{
			AimDir = Vector3.Normalize(target - Pos) * Length;
		}
	}

	const int LOOP_COUNT = 10000;
	
	// Use this for initialization
	void Start () 
	{
		AimingData aimingData = new AimingData(LOOP_COUNT, 10);

		TestObj[] testObj = new TestObj[LOOP_COUNT];
		for(int i=0; i< testObj.Length; ++i)
		{
			testObj[i] = new TestObj();
		}

		Vector3 target = new Vector3(100, 100, 100);


		PerformanceChecker sinPerfChecker = new PerformanceChecker("DOD vs OOP", 1);

		sinPerfChecker.CheckAction ( "DOD", (i) => 
		{
			CalcAim(aimingData, target, LOOP_COUNT);
		} );

		sinPerfChecker.CheckAction ( "OOP", (i) => 
		{
			for(int j=0; j<LOOP_COUNT; ++j)
			{
				testObj[j].Aim(target);
			}
		});

		sinPerfChecker.ShowResult();

		System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
		sw.Reset();
		sw.Start();
		CalcAim(aimingData, target, LOOP_COUNT);
		sw.Stop();
		Debug.Log("dod : "+ sw.ElapsedMilliseconds);

		sw.Reset();
		sw.Start();
		for (int j = 0; j < LOOP_COUNT; ++j)
		{
			testObj[j].Aim(target);
		}
		sw.Stop();
		Debug.Log("oop : " + sw.ElapsedMilliseconds );

	}

}