using UnityEngine;

namespace DiTreeGroup.Example
{
    public class BattleManagerView : ExampleClassBase<BattleManagerView>
    {
        public override void Run()
        {
            var logicManager = _tree.Get<BattleManagerLogic>();
            Debug.Log($"ViewManagerが {logicManager.Id}取得成功");
            /*
            var tree = this.CreateDiTree();
            var data = tree.Get<ExampleDataB>();
            Debug.Log(data.DataB);
            */
        }
    }
}