using gipo.core;
using gipo.util;

public class DbgObj {
	public int debugNum;
}

public class TestScene : GearHolder {

	public DbgObj dbgObj;

	[Diffuse]
	private TestLogic logic;

	[Diffuse]
	private TestView view;

	private void prepare() {
		UnityEngine.Debug.Log("TestScene : prepare");
		if (!_autoDiffuse) {
			_gear.Diffuse(logic, typeof(TestLogic));
			_gear.Diffuse(view, typeof(TestView));
		}
	}

	private void run() {
		UnityEngine.Debug.Log("TestScene : run");
	}

	private void bubble() {
		UnityEngine.Debug.Log("TestScene : bubble");
	}

	private void disposeProcess() {
		UnityEngine.Debug.Log("TestScene : disposeProcess");
	}

	public TestScene() : base(true) {
		dbgObj = new DbgObj();

		//（Gearのセットアップ後）初期化時に行いたいこと（Action）を追加
		// Attribute属性を用いていない場合はここでdiffuse/absorbをAction化しておくといい感じ？
		_gear.AddPreparationHandler(prepare, new PosInfos());

		// prepare後に行いたいこと（Action）を追加
		_gear.AddRunHandler(run, new PosInfos());

		// run後に行いたいこと（Action）を追加
		_gear.AddBubbleHandler(bubble, new PosInfos());

		// dispose時に行いたいこと（Action）を追加
		_gear.AddDisposeProcess(disposeProcess, new PosInfos());
	}

	protected override void FieldSetup() {
		logic = new TestLogic();
		view = new TestView();
	}

	// ==== test method ====
	public void debugAbsorb() {
		view.debugAbsorb();
	}
}
