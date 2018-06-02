using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GaviPractice.TouchControllerPanel
{
	public class TestCtrlObj : BaseTouchControlledObject
	{
		public override void SendDeltaPos(Vector3 ctrlPos)
		{
			transform.position += ctrlPos;
		}

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}
	}

}
