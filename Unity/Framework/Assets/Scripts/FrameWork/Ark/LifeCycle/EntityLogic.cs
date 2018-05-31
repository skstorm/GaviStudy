using System.Text;
using UnityEngine;

//#pragma warning disable 0067

namespace Ark.LifeCycle
{
	// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
	//! 外部アクセス用（読み込みOnly）
	public interface IEntityLogicPeek
	{
		Vector2 Pos { get; }
		float Radius { get; }
		IEntityViewOrder BaseView { get; }
		bool IsWillRemove { get; }
		ObjectCategory ObjectCategory { get; }
		ELifeCycleKind LifeCycleKind { get; }
	}

	// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
	//! 外部アクセス用(書き込み)
	public interface IEntityLogic : IEntityLogicPeek
	{
		void Init(IEntityFieldLogic_ForLifeCycle entityFieldLogic, IEntityViewOrder view, ELifeCycleKind lifeCycleKind);
		void Start();
		void Stop();
		void Release();
		void ChangeView(IEntityViewOrder view);
	}

	// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
	//! その他 peek
	public interface IEtcObjectPeek : IEntityLogicPeek
	{
		EtcObjectKind Kind { get; }
	}

	public interface IVisualObjectPeek : IEntityLogicPeek
	{
		VisualObjectKind Kind { get; }

		int CardOrder { get; }
	}

	public interface IFieldStructureObjectPeek : IEntityLogicPeek
	{
		FieldStructureObjectKind Kind { get; }
	}

	// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
	//! その他 order
	public interface IEtcObject : IEntityLogic, IEtcObjectPeek
	{

	}

	public interface IVisualObject : IEntityLogic, IVisualObjectPeek
	{

	}

	public interface IFieldStructureObject : IEntityLogic, IFieldStructureObjectPeek
	{
	}

	// 自身の区分(攻撃時のチェックに使用)
	public enum ObjectCategory
	{
		FieldStructure,
		Unit,
		Effect,
		Etc,
		Castle,
	}

	public enum FieldStructureObjectKind
	{
		TacticsPlant,  // 戦術拠点
		SecretPlanSwitchPlant, // 秘策スイッチ用拠点
	}

	public enum EtcObjectKind
	{
		Arrow = 0, // 弓矢
		VigorBall, // 士気玉
		ThrowStone, // 投石
		PlantFloor,     // 拠点の床
		SkillDamage,//スキル
		DeckArrow,  // デッキから伸びる矢印
		Bullet, // 銃弾
		SkillCountDown,
		LaserCharge,
		Turret,
		Bolt,
		Convex, // 
		Moses,
		ShockWave,
		ShockWaveGenerator,
		MosesGenerator,
	}

	// 顔アイコンの種類
	public enum VisualObjectKind
	{
		// 武将アイコンの種類
		Icon_SmallName, // 小さめの文字アイコン
		Icon_Skill,	// スキルアイコンver(便宜上SkillIconとしているが、他の種類が削除されたらIconにする)

		TieArrow,   // 武将に追随する矢印

		PlayerThumbnail, // 文字アイコンに追随するサムネイル画像
		EnemyThumbnail,
	}

	public enum UnitKind
	{
		Lancer = 0, // 槍兵
		Archer, // 弓兵
		Cavalry, // 騎兵
		Shosha,  // 衝車
		Tosekisha,// 投石車
		Pawn,
		CommanderLancer, // 武将　槍
		CommanderCavalry, // 武将・騎兵
		CommanderArcher, // 武将　弓
		CommanderGunner, // 武将・ガンナー
		CastleArcher, // 城を守る弓兵
		Gunner,
		CommanderShosha, // 武将　衝車
		CommanderTosekisha, // 武将　投石車
		CommanderEuropeanKnight,	// 西洋汎用武将
		Max,
	}

	public class EntityLogic<TEntityFieldLogic, TView, TState> : IEntityLogic, IUpdatable where TEntityFieldLogic : class, IEntityFieldLogic_ForEntityLogic where TView : class, IEntityViewOrder
	{
		/*
		private static IBattleLifeCycleQueContainer_Check s_syncQueContainer = null;
		protected IBattleLifeCycleQueContainer_Check SyncQueContainer
		{
			get { return s_syncQueContainer; }
		}

		private static SimpleLifeCycleQueContainer s_localQueContainer = null;
		public SimpleLifeCycleQueContainer LocalQueContainer
		{
			get { return s_localQueContainer; }
		}

		private static SimpleLifeCycleQueContainer s_alwaysQueContainer = null;
		protected SimpleLifeCycleQueContainer AlwaysQueContainer
		{
			get { return s_alwaysQueContainer; }
		}

		public static void StaticInit(IBattleLifeCycleQueContainer_Check syncQueContainer, SimpleLifeCycleQueContainer localQueContainer, SimpleLifeCycleQueContainer alwaysQueContainer)
		{
			s_syncQueContainer = syncQueContainer;
			s_localQueContainer = localQueContainer;
			s_alwaysQueContainer = alwaysQueContainer;
		}

		public static void StaticRelease()
		{
			s_syncQueContainer = null;
			s_localQueContainer = null;
			s_alwaysQueContainer = null;
		}
		*/
		// 座標
		protected Vector2 _pos;
		// View
		protected TView _view;
		// 現在状態
		protected TState _currentState;
		// 前の状態
		protected TState _prevState;
		// あたり半径
		protected float _radius;
		// 状態変更の予約が入っているかどうか
		private bool _isWillChangeState = false;
		// 状態変更をする状態
		private TState _willChangeState;
		// 消せるかどうか
		protected bool _isWillRemove = false;
		public bool IsWillRemove
		{
			get { return _isWillRemove; }
		}
		// 消すフラグを書き換える。危険なので専用のインターフェース（現状はIBattleUnitForMinionProcesserのみ）から実行して下さい。
		public void SetIsWillRemove(bool isWillRemove)
		{
			_isWillRemove = isWillRemove;
		}
		// バトルロジック
		protected TEntityFieldLogic _entityFieldLogic = null;
		// 自分がどのライフサイクルに入っているか判定するための変数
		protected ELifeCycleKind _lifeCycleKind = ELifeCycleKind.SyncBattle;
		public ELifeCycleKind LifeCycleKind
		{
			get { return _lifeCycleKind; }
		}
		// Viewを変更するかどうか
		protected bool _isWillChangeView = false;
		public bool IsWillChangeView
		{
			get { return _isWillChangeView; }
		}
		// 変更するView
		protected IEntityViewOrder _willChangeView = null;

		public IEntityViewOrder BaseView
		{
			get { return _view; }
		}
		public TView View
		{
			get { return _view; }
		}

		public Vector2 Pos
		{
			get { return _pos; }
		}

		public float Radius
		{
			get { return _radius; }
		}

		public TState CurrentState
		{
			get { return _currentState; }
		}

		// 壁とのチェックを行うか
		protected bool _isCheckWall = true;
		public bool IsCheckWall
		{
			get { return _isCheckWall; }
		}

		protected ObjectCategory _objectCategory = ObjectCategory.Etc;
		public ObjectCategory ObjectCategory { get { return _objectCategory; } }

		protected static StringBuilder s_stringBuilder = new StringBuilder();

		protected const float PUSH_RANGE_ZERO = 0.0f;

		public EntityLogic(int myCamp, float x, float y)
		{
			_radius = 35.0f;
			// 座標設定
			_pos.x = x;
			_pos.y = y;
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 状態を変更する
		public void ChangeState(TState newState)
		{
			_prevState = _currentState;
			ExitState(_currentState);
			_currentState = newState;
			EnterState(_currentState);
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! Viewを設定する
		public void SetView(IEntityViewOrder view)
		{
			_view = view as TView;
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 初期化 
		// Objectが生成されてGameLogicに追加される時に呼ばれる
		// ViewOrderは使えない
		virtual public void Init(IEntityFieldLogic_ForLifeCycle entityFieldLogic, IEntityViewOrder view, ELifeCycleKind lifeCycleKind)
		{
			SetView(view);
			_entityFieldLogic = entityFieldLogic as TEntityFieldLogic;
			_lifeCycleKind = lifeCycleKind;
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! ViewOrderが使えるタイミング
		virtual public void Start()
		{

		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 停止させる
		virtual public void Stop()
		{
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 解除される際の処理
		virtual public void Release()
		{
			_entityFieldLogic = null;
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 毎フレの更新処理　継承して使う
		virtual public void Update()
		{
		}
		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 一定フレーム間隔で行われる処理　継承して使う
		virtual public void PeriodicUpdate()
		{
		}
		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 状態に入る時の処理　継承して使う
		virtual protected void EnterState(TState state)
		{

		}
		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 状態を抜ける時の処理　継承して使う
		virtual protected void ExitState(TState state)
		{

		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! ユニットを壁の中に閉じ込める
		public virtual void ConfineInWall(float leftWall, float topWall, float rightWall, float bottomWall)
		{
			if (_pos.x < leftWall)
			{
				_pos.x = leftWall;
			}
			else if (_pos.x > rightWall)
			{
				_pos.x = rightWall;
			}

			if (_pos.y > topWall)
			{
				_pos.y = topWall;
			}
			else if (_pos.y < bottomWall)
			{
				_pos.y = bottomWall;
			}
		}
		
		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 変更するViewを取得する
		public IEntityViewOrder GetWillChangeView()
		{
			return _willChangeView;
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! Viewを変更する
		public void ChangeView(IEntityViewOrder view)
		{
			_willChangeView = view;
			_isWillChangeView = true;
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! View変更が終わる
		public void EndChangeView()
		{
			_isWillChangeView = false;
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 予約した状態の変更を行う
		public void UpdateChangeState()
		{
			if (_isWillChangeState)
			{
				ChangeState(_willChangeState);
				_isWillChangeState = false;
			}
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 状態変更の予約
		protected void BookChangeState(TState state)
		{
			_willChangeState = state;
			_isWillChangeState = true;
		}
	}
}