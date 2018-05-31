using Ark.Gear;
using Ark.Util;

namespace Ark.Core
{
	public interface ILogicStateChnager_ForView
	{
		void NotifyCommand(ICommand command);
	}

	public class LogicStateChanger : GearHolder, ILogicStateChnager_ForView
	{
		private IGameLogic_ForLogicStateChanger _gameLogic = null;

		public LogicStateChanger() : base(false)
		{
		}

		protected override void StartGearProcess()
		{
			base.StartGearProcess();

			_gameLogic = _gear.Absorb<GameLogic>(new PosInfos());
		}

		public void Update()
		{

		}

		public void NotifyCommand(ICommand command)
		{
			_gameLogic.NotifyCommand(command);
		}
	}
}