using gipo.core;
using gipo.util;

public class TestPanel : GearHolder {

	[Absorb]
	TestLogic logic = null;

	public TestPanel() : base(false) 
	{
	}

	// for debug
	public void debugAbsorb() {
		PosInfos posinfo;

		// field var
		if (_autoAbsorb) {
			posinfo = new PosInfos();
			logic.debug("logic.debug called : " + posinfo.ToString());
		}

		// local var
		posinfo = new PosInfos();
		TestLogic l_logic = _gear.Absorb<TestLogic>(posinfo);
		l_logic.debug("l_logic.debug called : " + posinfo.ToString());
	}
}
