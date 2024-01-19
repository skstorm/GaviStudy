using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public static class Localize
    {
        private static readonly Dictionary<ETextKind, Dictionary<ELanguageKind, string>> _dicText = new()
        {
            {
                ETextKind.TitleScene, new ()
                {
                    {ELanguageKind.Jp, "タイトルシーン" },
                    {ELanguageKind.En, "TitleScene" },
                    {ELanguageKind.Kr, "타이틀 씬" },
                }
            },

            {
                ETextKind.InGameScene, new ()
                {
                    {ELanguageKind.Jp, "ゲームシーン" },
                    {ELanguageKind.En, "InGameScene" },
                    {ELanguageKind.Kr, "게임 씬" },
                }
            },
        };

        public static ELanguageKind LanguageKind { get; set; }

        public static string Get(ETextKind textKind)
        {
            return _dicText[textKind][LanguageKind];
        }
    }
}