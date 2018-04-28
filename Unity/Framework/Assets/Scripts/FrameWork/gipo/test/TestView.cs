using gipo.core;
using gipo.util;

public class TestView : GearHolder {

	[Diffuse]
	private TestPanel panel;

	private void prepare() {
		UnityEngine.Debug.Log("TestPanel : prepare");
		if (!_autoDiffuse) {
			_gear.Diffuse(panel, typeof(TestPanel));
		}
	}

	private void run() {
		UnityEngine.Debug.Log("TestPanel : run");
	}

	private void bubble() {
		UnityEngine.Debug.Log("TestPanel : bubble");
	}

	private void disposeProcess() {
		UnityEngine.Debug.Log("TestPanel : disposeProcess");
	}

	public TestView() : base(false) 
	{
		_gear.AddPreparationHandler(prepare, new PosInfos());
		_gear.AddRunHandler(run, new PosInfos());
		_gear.AddBubbleHandler(bubble, new PosInfos());
		_gear.AddDisposeProcess(disposeProcess, new PosInfos());
	}

	protected override void FieldSetup() {
		panel = new TestPanel();
	}

	// for debug
	public void debugAbsorb() {
		panel.debugAbsorb();
	}
}
