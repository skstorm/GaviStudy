namespace Ark.LifeCycle
{
	public interface IEntityFieldLogic_ForEntityLogic
	{

	}

	public interface IEntityFieldLogic_ForLifeCycle
	{

	}

	public class EntityFieldLogic : IEntityFieldLogic_ForLifeCycle, IEntityFieldLogic_ForEntityLogic
	{
		public EntityFieldLogic()
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