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
		Run,
		Dispose,
	};

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

		private GearDispatcher _preparationDispatcher;
		private GearDispatcher _startDispatcher;
		private GearDispatcher _endDispatcher;

		/// コンストラクタ
		public Gear(IGearHolder holder) 
		{
			_holder = holder;
			_childGearList = new List<Gear>();
			_diffuser = new Diffuser(this);

			_phase = GearPhase.Create;

			_preparationDispatcher = new GearDispatcher(AddBehavior.MethodType.addTail, true, new PosInfos());
			_startDispatcher = new GearDispatcher(AddBehavior.MethodType.addTail, true, new PosInfos());
			_endDispatcher = new GearDispatcher(AddBehavior.MethodType.addHead, true, new PosInfos());
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
			case GearPhase.Run:
			case GearPhase.Dispose:
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
			case GearPhase.Run:
			case GearPhase.Dispose:
				return false;
			}
			throw new Exception("存在しないGearPhaseにいます");
		}

		public bool CheckPhaseCanAbsorb() 
		{
			switch (_phase) 
			{
			case GearPhase.Preparation:
			case GearPhase.Run:
				return true;
			case GearPhase.Create:
			case GearPhase.Dispose:
				return false;
			}
			throw new Exception("存在しないGearPhaseにいます");
		}

		public bool CheckPhaseRun() 
		{
			switch (_phase) 
			{
			case GearPhase.Run:
				return true;
			case GearPhase.Create:
			case GearPhase.Preparation:
			case GearPhase.Dispose:
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
			case GearPhase.Run:
				return true;
			case GearPhase.Dispose:
				return false;
			}
			throw new Exception("存在しないGearPhaseにいます");
		}

		// ==== 以下、handlerへの追加処理 ====
		public void AddPreparationProcess(Action func, PosInfos pos) 
		{
			_preparationDispatcher.add(func, pos);
		}

		public void AddStartProcess(Action func, PosInfos pos) 
		{
			_startDispatcher.add(func, pos);
		}

		public CancelKey AddEndProcess(Action func, PosInfos pos) 
		{
			if (!CheckPhaseBeforeDispose()) 
			{
				throw new Exception("既に消去処理が開始されているため、消去時のハンドラを登録できません" + _phase);
			}
			return _endDispatcher.add(func, pos);
		}

		public void Initialize() 
		{
			if (!CheckPhaseCreate()) return;

			_phase = GearPhase.Preparation;
			_preparationDispatcher.execute(new PosInfos());

			_phase = GearPhase.Run;
			// タスクが無くなったらrun実行
			_startDispatcher.execute(new PosInfos());
			_startDispatcher = null;

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
			if (_phase != GearPhase.Dispose) 
			{
				if (isDisposing) 
				{
					//
					_holder = null;
					foreach (var childGear in _childGearList) 
					{
						childGear.Dispose();
					}

					_endDispatcher.execute(new PosInfos());// 逆順で実行する
					_endDispatcher = null;

					_childGearList.Clear();
					_diffuser.Dispose();
				}
				_phase = GearPhase.Dispose;
			}
		}

		// for debug
		public void DebugPrint(string str) 
		{
			UnityEngine.Debug.Log("[" + str + "]:" + this._holder + "(" + _phase + ")");
		}
	}
}
