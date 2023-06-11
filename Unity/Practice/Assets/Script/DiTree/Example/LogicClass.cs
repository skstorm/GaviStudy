using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiTreeGroup.Example
{
    public class BattleManagerLogic : ExampleClassBase<BattleManagerLogic>
    {
        public string Id => "BattleSystemManager";

        private List<BattleCharaLogic> _characterList = new();

        public override void SetupTree()
        {
            var currentNode = _tree.GetCurrentNode();
            _tree.AddNode(currentNode, typeof(BattleCharaLogic));
            _characterList.Add(new BattleCharaLogic(1));
            _characterList.Add(new BattleCharaLogic(2));
            _characterList.Add(new BattleCharaLogic(3));
        }

        public override void Run()
        {
            foreach (var chara in _characterList)
            {
                chara.Run();
            }
            
            var viewManager = _tree.Get<BattleManagerView>();
            if (viewManager == null)
            {
                Debug.Log($"LogicManagerからViewManagerの取得失敗");
            }
        }
    }
    
    public class BattleCharaLogic : ExampleClassBase<BattleCharaLogic>
    {
        public readonly int CharaId;

        public BattleCharaLogic(int charaId)
        {
            CharaId = charaId;
        }

        public override void Run()
        {
            var logicManager = _tree.Get<BattleManagerLogic>();
            Debug.Log($"Chara({CharaId}) {logicManager.Id}取得成功");
        }
    }
}