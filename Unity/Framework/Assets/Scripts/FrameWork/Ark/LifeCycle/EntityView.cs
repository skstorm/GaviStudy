using UnityEngine;
using Ark.Util;

namespace Ark.LifeCycle
{
	public interface IEntityViewOrder
	{
		void Init(IEntityFieldView_ForEntityView entityFieldView, IEntityLogicPeek entity);
		void InitView(Transform parentTransform);
		void UpdateView( int deltaFrame);
		void ActiveView();
		void DeactiveView();
	}

	public interface IEntityView : IEntityViewOrder
	{
		GameObject GameObject { get; }
		Transform Transform { get; }
		bool IsPool { get; }
		bool IsViewActive { get; }
	}

	abstract public class BattleObjectView : MonoBehaviour, IEntityView
	{
		protected IEntityLogicPeek _entityLogic = null;

		protected IEntityFieldView_ForEntityView _entityFieldView;

		protected new Transform transform;
		public Transform Transform
		{
			get { return transform; }
		}

		// プール用かどうかを判別するための変数　trueだったら使いまわしする意味
		protected bool _isPool = true;
		public bool IsPool
		{
			get { return _isPool; }
		}

		public GameObject GameObject
		{
			get { return gameObject; }
		}

		protected bool _isViewActive = false;
		public bool IsViewActive
		{
			get { return _isViewActive; }
		}

		virtual public void Init(IEntityFieldView_ForEntityView entityFieldView, IEntityLogicPeek entity)
		{
			_entityFieldView = entityFieldView;

			InitPeek(entity);

			// 座標初期化
			InitPosition();

			InitRotation();
		}

		protected virtual void InitPeek(IEntityLogicPeek entity)
		{
			_entityLogic = entity;
		}

        protected virtual void InitPosition()
        {
        }

		protected virtual void InitRotation()
		{
		}

		public virtual void UpdateView( int deltaFrame)
		{
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! Viewインスタンスが生成される時に一回だけ行われる処理
		public virtual void InitView(Transform parentTransform)
		{
#if UNITY_EDITOR
			Renderer renderer = GetComponent<Renderer>();
			if (renderer != null)
			{
				CommonFunc.ReSetShaderForAssetBundle(renderer);
			}

			CommonFunc.ReSetShaderOfChildRenderer(gameObject.transform);
#endif

			transform = GetComponent<Transform>();
			transform.SetParent(parentTransform, false);
			transform.localScale = Vector3.one;
		}

		public void ActiveView()
		{
			gameObject.SetActive(true);
			_isViewActive = true;
		}

		public virtual void DeactiveView()
		{
			gameObject.SetActive(false);
			ReleasePeek();
			_isViewActive = false;
			_entityFieldView = null;
			/*	
			transform.localScale = Vector3.zero;
			gameObject.SetActive(true);
			*/
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! ピークの初期化
		protected virtual void ReleasePeek()
		{
			_entityLogic = null;
		}
	}
}