using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GaviPractice.TouchControllerPanel
{
	public class TestCtrlObj : BaseTouchControlledObject
	{
		Vector3 _originPos = Vector3.zero;

		public override void SendLocalCtrlPos(Vector3 ctrlPos)
		{
			transform.position = _originPos + ctrlPos;
		}

		// Use this for initialization
		void Start()
		{
			_originPos = transform.position;
		}

		// Update is called once per frame
		void Update()
		{

		}
	}

}
