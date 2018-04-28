namespace gipo.core
{
	public interface IGearHolder 
	{
		void GearInit();
		void AutoAbsorb(bool isRoot);
		Gear GetGear();
		void GearDispose();
		string GearDILog();
	}
}
