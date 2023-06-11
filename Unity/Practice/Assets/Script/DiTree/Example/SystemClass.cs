using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiTreeGroup.Example
{
    public class BattleSystemManager : ExampleClassBase<BattleSystemManager>
    {
        public string Id => "BattleSystemManager";

        private List<BattleSystemChara> _characterList = new();

        public override void SetupTree()
        {
            var currentNode = _tree.GetCurrentNode();
            _tree.AddNode(currentNode, typeof(BattleSystemChara));
            currentNode.RegisterInstance(this);

            _characterList.Add(new BattleSystemChara(1));
            _characterList.Add(new BattleSystemChara(2));
            _characterList.Add(new BattleSystemChara(3));
        }

        public override void Run()
        {
            var dataA = _tree.Get<ExampleDataA>();
            Debug.Log(dataA.DataA);

            foreach (var chara in _characterList)
            {
                chara.Run();
            }
        }
    }
    
    public class BattleSystemChara : ExampleClassBase<BattleSystemChara>
    {
        public readonly int CharaId;

        public BattleSystemChara(int charaId)
        {
            CharaId = charaId;
        }

        public override void Run()
        {
            var systemManager = _tree.Get<BattleSystemManager>();
            Debug.Log($"Chara({CharaId}) {systemManager.Id}取得成功");
        }
    }
}