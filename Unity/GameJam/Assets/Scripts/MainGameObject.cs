using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameJam
{
    public class MainGameObject : MonoBehaviour
    {
        public async UniTask Run(StateMachine stateMachine, BaseStateBehaviour startState)
        {
            Localize.LanguageKind = ELanguageKind.En;
            Application.targetFrameRate = 60;

            DontDestroyOnLoad(this);

            await stateMachine.RunAsync(startState);
            stateMachine.Release();
        }
    }
}