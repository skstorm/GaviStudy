namespace gipo.core
{
	public interface IGearHolder 
	{
		void GearInit();
		Gear GetGear();
		void GearDispose();
		string GearDILog();
	}
}
