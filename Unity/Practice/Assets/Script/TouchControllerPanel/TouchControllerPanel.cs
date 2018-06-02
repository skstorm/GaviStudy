using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

namespace GaviPractice.TouchControllerPanel
{
	public class BaseTouchControlledObject : MonoBehaviour
	{
		public virtual void SendDeltaPos(Vector3 deltaPos)
		{

		}
	}
	

	public class TouchControllerPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		// 現在座標
		private Vector3 _curPos;
		// 前の座標
		private Vector3 _prevPos;
		// 座標差分
		private Vector3 _deltaPos;
		// 制御した座標の最終位置（累積されている）
		[SerializeField]
		private Vector3 _ctrlPos = Vector3.zero;
		// 移動の比率（高いほど早く動く）
		[SerializeField]
		private float _moveRate = 1;
		// 影響を受けるObject
		[SerializeField]
		private List<BaseTouchControlledObject> _ctrlObjList = null;
		// 滑るのを判定するためのフラグ
		[SerializeField]
		private bool _isSlip = false;
		// スリップ摩擦
		[SerializeField]
		private float _slipFriction = 0.8f;
		// 移動タイプ
		private enum EMoveKind
		{
			Horizontal = 0,
			Vertical,
			Both
		}
		[SerializeField]
		private EMoveKind _moveKind = EMoveKind.Both;
		// TouchControllerPanelが制御可能な状態なのか
		private bool _isCanCtrl = false;
		// 動く力がこの力以下の力だったら、動きを止める
		[SerializeField]
		private float _stopForceCutLine = 0.0001f;

		public void OnPointerDown(PointerEventData eventData)
		{
			Debug.Log("Down");
			Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _moveRate));
			_prevPos = _curPos = pos;
			_isSlip = false;
			_isCanCtrl = true;
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			Debug.Log("Up");
			_isSlip = true;
		}

		void Update()
		{
			if(!_isCanCtrl)
			{
				return;
			}

			// タッチ中の処理
			if (Input.GetMouseButton(0))
			{
				// 現在座標を取得
				_curPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _moveRate));
				// 差分座標を計算
				_deltaPos = Vector3.zero;
				// 移動種類によって差分座標を更新する
				switch(_moveKind)
				{
					case EMoveKind.Horizontal: _deltaPos.x = _curPos.x - _prevPos.x; break;
					case EMoveKind.Vertical: _deltaPos.y = _curPos.y - _prevPos.y; break;
					case EMoveKind.Both:
						{
							_deltaPos.x = _curPos.x - _prevPos.x;
							_deltaPos.y = _curPos.y - _prevPos.y;
						}
						 break;
				}
				_deltaPos.z = _curPos.z - _prevPos.z;
				// 座標関係作業の最後処理
				ProcessPos(_deltaPos);
			}

			// 滑る処理
			if (_isSlip)
			{
				_deltaPos *= _slipFriction;
				// 座標関係作業の最後処理
				ProcessPos(_deltaPos);
				// 動く力が一定以下だったら止める
				float moveForce = _deltaPos.sqrMagnitude;
				if (moveForce <= _stopForceCutLine)
				{
					_isSlip = false;
					_isCanCtrl = false;
				}
			}
		}

		// 座標関係作業の最後処理
		void ProcessPos(Vector3 deltaPos)
		{
			_ctrlPos += deltaPos;
			_prevPos = _curPos;
			for (int i = 0; i < _ctrlObjList.Count; ++i)
			{
				_ctrlObjList[i].SendDeltaPos(_deltaPos);
			}
		}
	}

}