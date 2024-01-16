using System.Diagnostics;
using UnityEngine;

namespace GameJam
{
    public class Util
    {
        public static T LoadScenePrefab<T>(string path, IStateMachine fsm) where T : BaseStateBehaviour
        {
            var origin = Resources.Load<T>(path);
            var obj = GameObject.Instantiate<T>(origin);
            obj.Init(fsm);
            return obj;
        }

        [Conditional("GAME_JAM_DEBUG")]
        public static void DebugLog(string log)
        {
#if GAME_JAM_DEBUG
            UnityEngine.Debug.Log(log);
#endif // GAME_JAM_DEBUG
        }
    }
}