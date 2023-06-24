using UnityEngine;

namespace DiTreeGroup.Example
{
    /// <summary>
    /// 動作確認用クラス
    /// </summary>
    public class BattleManagerView : ExampleClassBase<BattleManagerView>
    {
        public override void Run()
        {
            var logicManager = _tree.Get<BattleManagerLogic>();
            Debug.Log($"ViewManagerが {logicManager.Id}取得成功");
            /*
             treeを予め保持しなくても、使うときに生成するのも可能↓
            var tree = this.CreateDiTree();
            var data = tree.Get<ExampleDataB>();
            Debug.Log(data.DataB);
            */
        }
    }
}