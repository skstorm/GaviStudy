using System.Collections.Generic;
using UnityEngine;

namespace DiTreeGroup.Example
{
    /// <summary>
    /// 動作確認用クラス
    /// </summary>
    public class BattleManagerLogic : ExampleClassBase<BattleManagerLogic>
    {
        /// <summary>Id</summary>
        public string Id => "BattleManagerLogic";

        /// <summary>キャラリスト</summary>
        private List<BattleCharaLogic> _characterList = new();

        /// <summary>
        /// Treeセットアップ
        /// </summary>
        public override void SetupTree()
        {
            // Treeから現在自分のノード取得
            var currentNode = _tree.GetCurrentNode();
            
            // 自分のノードの下にキャラロジッククラスを追加
            _tree.AddNode(currentNode, typeof(BattleCharaLogic));
            
            // リストにキャラ追加
            _characterList.Add(new BattleCharaLogic(1));
            _characterList.Add(new BattleCharaLogic(2));
            _characterList.Add(new BattleCharaLogic(3));
        }

        /// <summary>
        /// 実行
        /// </summary>
        public override void Run()
        {
            foreach (var chara in _characterList)
            {
                chara.Run();
            }
            
            // BattleManagerViewを取得してみる
            var viewManager = _tree.Get<BattleManagerView>();
            // BattleManagerViewはBattleManagerLogicノードからは取得できない位置に登録されているため失敗する
            if (viewManager == null)
            {
                Debug.Log($"LogicManagerからViewManagerの取得失敗");
            }
        }
    }
    
    /// <summary>
    /// 動作確認用クラス
    /// </summary>
    public class BattleCharaLogic : ExampleClassBase<BattleCharaLogic>
    {
        /// <summary>キャラId</summary>
        public readonly int CharaId;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public BattleCharaLogic(int charaId) : base()
        {
            CharaId = charaId;
        }

        /// <summary>
        /// 実行
        /// </summary>
        public override void Run()
        {
            // BattleManagerLogicを取得
            var logicManager = _tree.Get<BattleManagerLogic>();
            // BattleManagerLogicはBattleCharaLogicの親として登録されているので成功する
            Debug.Log($"Chara({CharaId}) {logicManager.Id}取得成功");
        }
    }
}