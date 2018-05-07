using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;

public class PerformanceChecker {

	List<Experiment> expList = new List<Experiment>();

	string mTestName;
	int mLoopCount;

	struct Experiment {
		public string name;
		public double elapsedMSec;
		public int loopCount;
		public long beforeMonoUsedKB;
		public long afterMonoUsedKB;
		public long diffMonoUsedKB;
	}

	public PerformanceChecker ( string testName, int loopCount) {
		mTestName = testName;
		mLoopCount = loopCount;

		//var a = UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong() / 1024;
	}

	void Prepare ( Action<int> targetFunc ) {
		// warming up
		targetFunc(0);

		// 直前のゴミを掃除
		GC.Collect();
		GC.WaitForPendingFinalizers();
	}

	/// <summary>
	/// 実際に、計測する所
	/// 注意：１回warmingUpしてからLoopCount回繰り返して、これを２回行います(合計2+2*LoopCount回実行される)
	/// </summary>
	/// <param name="actionName">Action name.</param>
	/// <param name="targetFunc">Target func.</param>
	public void CheckAction ( string actionName, Action<int> targetFunc ) 
	{

		Experiment exp = new Experiment();

		exp.name = actionName;
		exp.loopCount = mLoopCount;

		Prepare(targetFunc);

		// check time
		Stopwatch stopWatch = Stopwatch.StartNew();
		for ( int i = 0; i < mLoopCount; i++ ) {
			targetFunc(i);
		}
		stopWatch.Stop();

		exp.elapsedMSec = stopWatch.Elapsed.TotalSeconds * 1000;

		stopWatch.Reset();

		Prepare(targetFunc);

		// 時間計測とメモリ消費計算、同時にやるとお互いの結果に影響して邪魔なので、別々に区切って実施。
		// check memory
		exp.beforeMonoUsedKB = UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong() / 1024;

		for ( int i = 0; i < mLoopCount; i++ ) {
			targetFunc(i);
		}

		exp.afterMonoUsedKB = UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong() / 1024;
		exp.diffMonoUsedKB = exp.afterMonoUsedKB - exp.beforeMonoUsedKB;

		expList.Add(exp);
	}

	public void ShowResult () {
		StringBuilder result = new StringBuilder();

		AddHeader(result);

		for ( int i = 0; i < expList.Count; i++ ) {
			result.Append("| " +
				expList[i].name + " | " +
				expList[i].elapsedMSec + " | " + 
				( expList[i].afterMonoUsedKB - expList[i].beforeMonoUsedKB ) +
				" | \n" );
		}

		AddBottom(result);


		UnityEngine.Debug.Log(result.ToString());
	}

	void AddHeader (StringBuilder result) {
		result.Append(
			"# " + mTestName + "\n\n" +
			"- ループ回数:" + mLoopCount + "回実行 \n\n");

		result.Append("| Name | 時間(ms) | 消費メモリ(kb) |\n");
		result.Append("|:----:|:-------:|:-------------:|\n");
	}

	void AddBottom (StringBuilder result) {
		result.Append("\n");

		result.Append("## 結果\n\n");

		int mostFastestIndex = getMostFastestIndex();
		if ( mostFastestIndex == -1 ) {
			result.Append ( "- 早さは変わらない\n");
		} else {
			result.Append ( "- 早いのは 「" + expList[mostFastestIndex].name + "」 \n" );
		}

		int mostSmallestMemIndex = getMostSmallestMemoryIndex();

		if ( mostSmallestMemIndex == -1 ) {
			result.Append ( "- メモリ利用量は変わらない\n");
		} else {
			result.Append ( "- メモリ利用量が少ないのは 「" + expList[mostSmallestMemIndex].name + "」 \n" );
		}

		result.Append("\n");

		result.Append("## 確認したコード\n\n");

		result.Append("``` \n\n//ここに、確認したコードを貼付けて下さい\n\n```" );

		result.Append("\n\n\n");
	}

	/// <summary>
	/// Gets the index of the most fastest.
	/// </summary>
	/// <returns>
	/// The most fastest index.
	/// -1 means all values are same.
	/// </returns>
	int getMostFastestIndex () {
		if ( expList.Count == 0 ) {
			throw new Exception("before check faster, must call checkAction");
		}
		if ( expList.All ( (exp) => { return expList[0].elapsedMSec == exp.elapsedMSec; } ) ) {
			return -1;
		}

		double mostSmallestElapsedMSec = expList.Min ( (exp) => { return exp.elapsedMSec; } );

		for ( int i = 0; i < expList.Count; i++ ) {
			if ( expList[i].elapsedMSec == mostSmallestElapsedMSec ) {
				return i;
			}
		}
		return 0;
	}

	/// <summary>
	/// Gets the index of the most smallest memory.
	/// </summary>
	/// <returns>
	/// The most smallest memory index.
	/// -1 means all values are same.
	/// </returns>
	int getMostSmallestMemoryIndex () {
		if ( expList.Count == 0 ) {
			throw new Exception("before check faster, must call checkAction");
		}
		if ( expList.All( (exp) => { return expList[0].diffMonoUsedKB == exp.diffMonoUsedKB; } ) ) {
			return -1;
		}

		long mostSmallestMemory = expList.Min ( (exp) => { return exp.diffMonoUsedKB; } );

		for ( int i = 0; i < expList.Count; i++ ) {
			if ( expList[i].diffMonoUsedKB == mostSmallestMemory ) {
				return i;
			}
		}
		return 0;
	}
}