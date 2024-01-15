using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class Util
    {
        public static T LoadScenePrefab<T>(string path, IBaseFsm fsm) where T : BaseStateBehaviour
        {
            var origin = Resources.Load<T>(path);
            var obj = GameObject.Instantiate<T>(origin);
            obj.Init(fsm);
            return obj;
        }
    }
}