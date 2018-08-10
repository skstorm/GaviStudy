using Ark.LifeCycle;

namespace Example
{
	public interface ITestUnitLogicPeek : IEntityLogicPeek
	{
		TestUnitLogic.EKind Kind { get; }
	}

	public interface ITestUnitLogic : ITestUnitLogicPeek, IEntityLogic
	{
	}

	public class TestUnitLogic : EntityLogic<IEntityFieldLogic_ForEntityLogic, ITestUnitViewOrder, TestUnitLogic.EKind>, ITestUnitLogic
	{
		public enum EKind
		{
			NormalUnit = 0,
		}

		public EKind Kind { get { return EKind.NormalUnit; } }

		public TestUnitLogic(int myCampId, float x, float y) : base(myCampId,x, y)
		{

		}
	}
}