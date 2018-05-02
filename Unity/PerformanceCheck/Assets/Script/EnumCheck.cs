using UnityEngine;
using System.Collections;

public class EnumCheck : MonoBehaviour {

	enum State {
		XXX,
	}

	// Use this for initialization
	void Start () {

		State x = State.XXX;

		PerformanceChecker perfChecker = new PerformanceChecker("== vs Equals in Enum", 10000);
		perfChecker.CheckAction ( "==", (i) => {
			if (x == State.XXX ) {
			}
		});

		perfChecker.CheckAction ( "Equals", (i) => {
			if ( x.Equals(State.XXX) ){
			}
		});

		perfChecker.ShowResult();
	}
}