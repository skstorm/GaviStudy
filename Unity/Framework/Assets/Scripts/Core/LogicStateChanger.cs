using gipo.core;
using gipo.util;

namespace Core
{
	public interface ILogicStateChnagerForView
	{
		void NotifyCommand(ICommand command);
	}

	public class LogicStateChanger : GearHolder, ILogicStateChnagerForView
	{
		private IGameLogicForLogicStateChanger _gameLogic = null;

		public LogicStateChanger() : base(false)
		{
		}

		protected override void Run()
		{
			base.Run();

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