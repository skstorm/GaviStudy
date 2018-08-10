using Ark.LifeCycle;

namespace Example
{
	public interface ITestUnitViewOrder : IEntityViewOrder
	{
	}

	public interface ITestUnitView : ITestUnitViewOrder, IEntityView
	{

	}

	public class TestUnitView : EntityView, ITestUnitView
	{

	}
}