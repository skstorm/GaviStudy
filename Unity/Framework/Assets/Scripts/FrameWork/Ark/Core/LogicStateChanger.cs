using Ark.DiTree;
using Ark.Gear;
using Ark.Util;

namespace Ark.Core
{
	public interface ILogicStateChnager_ForView
	{
		void NotifyCommand(ICommand command);
	}

	public class LogicStateChanger : ArkDiTreeHolder<LogicStateChanger>, ILogicStateChnager_ForView
	{
		private IGameLogic_ForLogicStateChanger _gameLogic = null;

		public LogicStateChanger() : base(true)
		{
		}

		protected override void StartNodeProcess()
		{
			base.StartNodeProcess();

			_gameLogic = _tree.Get<GameLogic>();
		}

		public override void Run()
		{
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