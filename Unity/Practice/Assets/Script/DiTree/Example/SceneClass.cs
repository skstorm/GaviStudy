using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiTreeGroup.Example
{
    public class BattleSceneManager : ExampleClassBase<BattleSceneManager>
    {
        public override void Run()
        {
            var systemManager = _tree.Get<BattleSystemManager>();
            Debug.Log($"SceneManagerが {systemManager.Id}取得成功");
            /*
            var tree = this.CreateDiTree();
            var data = tree.Get<ExampleDataB>();
            Debug.Log(data.DataB);
            */
        }
    }
}