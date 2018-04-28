using gipo.core;
using gipo.util;

public class TestLogic : GearHolder {

	[Diffuse]
	private TestService service;

	private void prepare() {
		UnityEngine.Debug.Log("TestLogic : prepare");
		if (!_autoDiffuse) {
			_gear.Diffuse(service, typeof(TestService));
		}
	}

	private void run() {
		UnityEngine.Debug.Log("TestLogic : run");
	}

	private void bubble() {
		UnityEngine.Debug.Log("TestLogic : bubble");
	}

	private void disposeProcess() {
		UnityEngine.Debug.Log("TestLogic : disposeProcess");
	}

	public TestLogic() : base(false) 
	{
		_gear.AddPreparationHandler(prepare, new PosInfos());
		_gear.AddRunHandler(run, new PosInfos());
		_gear.AddBubbleHandler(bubble, new PosInfos());
		_gear.AddDisposeProcess(disposeProcess, new PosInfos());
	}

	protected override void FieldSetup() {
		service = new TestService();
	}

	// ==== test method ====
	public void debug(string str) {
		UnityEngine.Debug.Log(str);
	}
}
