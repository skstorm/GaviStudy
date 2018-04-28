using gipo.core.handler;
using gipo.util;
using System;
using System.Collections.Generic;

namespace gipo.core
{
	public enum GearPhase 
	{
		Create,
		Preparation,
		Fulfill,
		Middle,
		Dispose,
		Invalid,
	};

	public enum GearNeedProcess 
	{
		Core,
	}

	/// diffuserを楽に扱えるようにするためのクラス
	/// こちらも循環参照からのメモリ管理を厳しくするためにIDisposableを持つ
	public class Gear : IDisposable 
	{

		/// このGearインスタンスを保持しているクラス（要IGearHolder）
		private IGearHolder _holder;
		/// 子Gearリスト
		private List<Gear> _childGearList;
		/// このギアを保持するインスタンス用のdiffuser
		public Diffuser _diffuser;

		/// handler
		private GearPhase _phase;

		private GearDispatcher _preparationHandlerList;
		private GearDispatcher _runDispatcher;
		private GearDispatcher _bubbleHandlerList;
		private GearDispatcher _disposeProcessStack;

		private Action _needProcessCore;
		private List<Action> _needProcesss;

		/// IDispose用フラグ
		private bool _disposed = false;

		/// コンストラクタ
		public Gear(IGearHolder holder) 
		{
			_holder = holder;
			_childGearList = new List<Gear>();
			_diffuser = new Diffuser(this);

			_phase = GearPhase.Create;

			_preparationHandlerList = new GearDispatcher(AddBehavior.MethodType.addTail, true, new PosInfos());
			_runDispatcher = new GearDispatcher(AddBehavior.MethodType.addTail, true, new PosInfos());
			_bubbleHandlerList = new GearDispatcher(AddBehavior.MethodType.addHead, true, new PosInfos());
			_disposeProcessStack = new GearDispatcher(AddBehavior.MethodType.addHead, true, new PosInfos());

			_needProcesss = new List<Action>();
			_needProcessCore = () => {};
			AddNeedProcess(_needProcessCore, new PosInfos());
		}

		// ==== 以下、diffuse/absorb ====

		/// 自身の子Gearを設定しつつ、子Diffuserの親に自身のDiffuserを設定させる
		public void AddChildGear(Gear gear) 
		{
			_childGearList.Add(gear); // 子Gearに追加
			gear._diffuser.setParent(_diffuser); // 子のdiffuserの親を自分に
		}

		// 子のDiffuserの親を空に設定したうえで、子Gearをリストからを削除
		public void RemoveChildGear(Gear gear) 
		{
			gear._diffuser.setParent(null); // 子のdiffuserの親を空に
			_childGearList.Remove(gear);
		}

		/// Gearの取得（Dispose後に使えないインスタンスを返さないようにしている）
		public Gear GetImplement() 
		{
			if (_holder == null) return null;
			return this;
		}

		/// diffuse。Diffuserにインスタンスを登録
		public void Diffuse(object diffuseInstance, Type clazz) 
		{
			_diffuser.add(diffuseInstance, clazz);
		}

		/// absorb。Diffuserから該当クラスのインスタンスを取得
		public T Absorb<T>(PosInfos pos) 
		{
			if (_holder == null) return default(T);
			return _diffuser.get<T>(pos);
		}

		// for Debug
		public string DILog() 
		{
			if (_holder == null) return "";
			string ret = "== " + _holder.GetType().FullName + "\n";
			ret += _diffuser.DILog();
			foreach (var childGear in _childGearList) {
				ret += childGear.DILog() + "\n";
			}
			return ret;
		}

		// ==== 以下、フェーズチェック ====
		public bool CheckPhaseCreate() 
		{
			switch (_phase) 
			{
			case GearPhase.Create:
				return true;
			case GearPhase.Preparation:
			case GearPhase.Fulfill:
			case GearPhase.Middle:
			case GearPhase.Dispose:
			case GearPhase.Invalid:
				return false;
			}
			throw new Exception("存在しないGearPhaseにいます");
		}

		public bool CheckPhaseCanPreparationTool() 
		{
			switch (_phase) 
			{
			case GearPhase.Preparation:
				return true;
			case GearPhase.Create:
			case GearPhase.Fulfill:
			case GearPhase.Middle:
			case GearPhase.Dispose:
			case GearPhase.Invalid:
				return false;
			}
			throw new Exception("存在しないGearPhaseにいます");
		}

		public bool CheckPhaseCanAbsorb() 
		{
			switch (_phase) 
			{
			case GearPhase.Preparation:
			case GearPhase.Fulfill:
			case GearPhase.Middle:
				return true;
			case GearPhase.Create:
			case GearPhase.Dispose:
			case GearPhase.Invalid:
				return false;
			}
			throw new Exception("存在しないGearPhaseにいます");
		}

		public bool CheckPhaseCanMiddleTool() 
		{
			switch (_phase) 
			{
			case GearPhase.Fulfill:
			case GearPhase.Middle:
				return true;
			case GearPhase.Create:
			case GearPhase.Preparation:
			case GearPhase.Dispose:
			case GearPhase.Invalid:
				return false;
			}
			throw new Exception("存在しないGearPhaseにいます");
		}

		public bool CheckPhaseBeforeDispose() 
		{
			switch (_phase) 
			{
			case GearPhase.Create:
			case GearPhase.Preparation:
			case GearPhase.Fulfill:
			case GearPhase.Middle:
				return true;
			case GearPhase.Dispose:
			case GearPhase.Invalid:
				return false;
			}
			throw new Exception("存在しないGearPhaseにいます");
		}

		// ==== 以下、handlerへの追加処理 ====
		public void AddPreparationHandler(Action func, PosInfos pos) 
		{
			_preparationHandlerList.add(func, pos);
		}

		public void AddRunHandler(Action func, PosInfos pos) 
		{
			_runDispatcher.add(func, pos);
		}

		public void AddBubbleHandler(Action func, PosInfos pos) 
		{
			_bubbleHandlerList.add(func, pos);
		}

		public CancelKey AddDisposeProcess(Action func, PosInfos pos) 
		{
			if (!CheckPhaseBeforeDispose()) 
			{
				throw new Exception("既に消去処理が開始されているため、消去時のハンドラを登録できません" + _phase);
			}
			return _disposeProcessStack.add(func, pos);
		}

		// ==== 以下、初期化処理 ====
		public void AddNeedProcess(Action key, PosInfos pos) 
		{
			foreach (var needProcess in _needProcesss) 
			{
				if (needProcess == key) 
				{
					throw new Exception("初期化タスクに２重登録されました");
				}
			}

			if (!CheckPhaseCreate()) 
			{
				throw new Exception("initializeProcessの追加はコンストラクタで行なって下さい");
			}

			_needProcesss.Add(key);
		}

		public void EndNeedProcess(Action key, PosInfos pos) 
		{
			_needProcesss.Remove(key);
			if (_needProcesss.Count != 0) return;

			// タスクが無くなったらrun実行
			_runDispatcher.execute(pos);
			_runDispatcher = null;

			// run後にbubble実行
			_bubbleHandlerList.execute(pos);
			_bubbleHandlerList = null;
		}

		public void Initialize() 
		{
			//DebugPrint("initialize");

			if (!CheckPhaseCreate()) return;

			_phase = GearPhase.Preparation;

			_preparationHandlerList.execute(new PosInfos());

			_phase = GearPhase.Fulfill;

			_phase = GearPhase.Middle;

			EndNeedProcess(_needProcessCore, new PosInfos());

			foreach (var childGear in _childGearList) 
			{
				childGear.Initialize();
			}
		}

		// ==== 以下、IDispose ====
		~Gear() 
		{
			this.Dispose(false);
		}

		public void Dispose() 
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool isDisposing) 
		{
			if (!_disposed) 
			{
				if (isDisposing) 
				{
					//
					_holder = null;
					foreach (var childGear in _childGearList) 
					{
						childGear.Dispose();
					}

					_disposeProcessStack.execute(new PosInfos());// 逆順で実行する
					_disposeProcessStack = null;

					_childGearList.Clear();
					_diffuser.Dispose();
				}
				_disposed = true;
			}
		}

		// for debug
		public void DebugPrint(string str) 
		{
			UnityEngine.Debug.Log("[" + str + "]:" + this._holder + "(" + _phase + ")");
		}
	}
}
