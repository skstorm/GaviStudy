using Ark.Gear;

namespace Ark.LifeCycle
{
	public interface IEntityFieldLogic_ForEntityLogic
	{

	}

	public interface IEntityFieldLogic_ForLifeCycle
	{

	}

	public class EntityFieldLogic : GearHolder, IEntityFieldLogic_ForLifeCycle, IEntityFieldLogic_ForEntityLogic
	{
		public EntityFieldLogic(): base(false)
		{

		}

		public void Init()
		{

		}

		public void Update(int deltaFrame)
		{

		}

		public void Release()
		{

		}
	}
}