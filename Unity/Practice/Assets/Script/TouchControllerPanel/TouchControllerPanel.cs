using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

namespace GaviPractice.TouchControllerPanel
{
	public class BaseTouchControlledObject : MonoBehaviour
	{
		public virtual void SendLocalCtrlPos(Vector3 deltaPos)
		{

		}
	}
	

	public class TouchControllerPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		// 現在座標
		private Vector3 _curPos = Vector3.zero;
		// 前の座標
		private Vector3 _prevPos = Vector3.zero;
		// 制御した座標の最終位置（累積されている）
		[SerializeField]
		private Vector3 _ctrlPos = Vector3.zero;
		// 差分座標
		private Vector3 _deltaPos = Vector3.zero;
		// 移動の比率（高いほど早く動く）
		[SerializeField]
		private float _moveRate = 1;
		// 影響を受けるObject
		[SerializeField]
		private List<BaseTouchControlledObject> _ctrlObjList = null;
		// スリップ摩擦
		[SerializeField]
		private float _slipFriction = 0.8f;
		// CtrlPosの可視化するためのObj（Debug用）
		[SerializeField]
		private GameObject _ctrlViewObj = null;
		// CtrlPosが動ける領域（左上段）
		[SerializeField]
		private Transform _leftTopLimitTrans = null;
		private Vector3 _leftTopLimitPos = Vector3.zero;
		// CtrlPosが動ける領域（右下段）
		[SerializeField]
		private Transform _rightBottomLimitTrnas = null;
		private Vector3 _rightBottomLimitPos = Vector3.zero;
		// 移動タイプ
		private enum EMoveKind
		{
			Horizontal = 0,
			Vertical,
			Both
		}
		[SerializeField]
		private EMoveKind _moveKind = EMoveKind.Both;
		// 動く力がこの力以下の力だったら、動きを止める
		[SerializeField]
		private float _stopForceCutLine = 0.0001f;
		// TouchControllerPanelの元のPos
		private Vector3 _originPos = Vector3.zero;
		// 状態
		private enum EState
		{
			None = 0,
			Move,
			Slip,
			Fit,
		}
		private EState _state = EState.None;

		public void OnPointerDown(PointerEventData eventData)
		{
			Debug.Log("Down");
			Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _moveRate));
			_prevPos = _curPos = pos;
			_state = EState.Move;
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			Debug.Log("Up");
			_state = EState.Slip;
		}

		void Start()
		{
			_originPos = transform.position;
			_ctrlPos = _originPos;
			_leftTopLimitPos = _leftTopLimitTrans.position;
			_rightBottomLimitPos = _rightBottomLimitTrnas.position;
		}

		void Update()
		{
			switch(_state)
			{
				case EState.None: break;
				case EState.Move: Move(); break;
				case EState.Slip: Slip(); break;
				case EState.Fit: Fit(); break;
			}
		}

		void Move()
		{
			// タッチ中の処理
			if (Input.GetMouseButton(0))
			{
				// 現在座標を取得
				_curPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _moveRate));
				// 差分座標を計算
				_deltaPos = Vector3.zero;
				// 移動種類によって差分座標を更新する
				switch (_moveKind)
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
		}

		void Slip()
		{
			_deltaPos *= _slipFriction;
			// 座標関係作業の最後処理
			ProcessPos(_deltaPos);
			// 動く力が一定以下だったら止める
			float moveForce = _deltaPos.sqrMagnitude;
			if (moveForce <= _stopForceCutLine)
			{
				_state = EState.None;
			}
		}

		void Fit()
		{

		}

		// 座標関係作業の最後処理
		void ProcessPos(Vector3 deltaPos)
		{
			// コントロール座標更新
			_ctrlPos += deltaPos;
			// コントロールを移動可能範囲内に収める
			if (_ctrlPos.x <= _leftTopLimitPos.x)
			{
				_ctrlPos.x = _leftTopLimitPos.x;
				Debug.Log("Left");
			}
			else if (_ctrlPos.x >= _rightBottomLimitPos.x)
			{
				_ctrlPos.x = _rightBottomLimitPos.x;
				Debug.Log("Right");
			}

			if (_ctrlPos.y <= _rightBottomLimitPos.y)
			{
				_ctrlPos.y = _rightBottomLimitPos.y;
				Debug.Log("Bottom");
			}
			else if (_ctrlPos.y >= _leftTopLimitPos.y)
			{
				_ctrlPos.y = _leftTopLimitPos.y;
				Debug.Log("top");
			}
			// コントロール座標可視化（Debug用）
			_ctrlViewObj.transform.position = _ctrlPos;
			// 現在の座標を前の座標に格納
			_prevPos = _curPos;
			// ローカルコントロール座標を計算
			Vector3 localCtrlPos = _ctrlPos - _originPos;
			for (int i = 0; i < _ctrlObjList.Count; ++i)
			{
				_ctrlObjList[i].SendLocalCtrlPos(localCtrlPos);
			}
		}
	}

}