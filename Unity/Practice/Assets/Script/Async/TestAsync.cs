using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

namespace Async
{
    public class TestAsync : MonoBehaviour
    {
        // Start is called before the first frame update
        async void OnEnable()
        {
            Debug.Log("Start");
            /*
            {
                int sum = sumFunc(5);
                Debug.Log($"Sum : { sum}");
            }
            */
            
            /*
            {
                var task1 = Task.Run(() => sumFunc(5));
                await task1;
                Debug.Log($"Sum : { task1.Result}");
            }
            */

            {
                var noodle = boilNoodle(2000, 10);
                var tamago = boilTamago(1000, 10);
                var soap = createSoap(2000, 10);

                 var tasks = new List<Task> { noodle, tamago, soap };
                // var tasks = new List<Task>();
                while(tasks.Count > 0)
                {
                    var task = await Task.WhenAny(tasks);
                    if(task == noodle)
                    {
                        Debug.Log("Boil Noddle !!!");
                    }
                    else if(task == tamago)
                    {
                        Debug.Log("Boil Tamago !!!");
                    }
                    else if (task == soap)
                    {
                        Debug.Log("Complete Soap !!!");
                    }
                    tasks.Remove(task);
                }

                Debug.Log("!!! All Complete !!!");
            }
        }

        private int sumFunc(in int num)
        {
            int result = 0;
            for(int i=0; i<num; ++i)
            {
                result += i;
                Thread.Sleep(1000);
            }
            return result;
        }

        private async Task<Boil> boilNoodle(int _msec, int _debugNum)
        {
            for(int i=0; i<_debugNum; ++i)
            {
                Debug.Log( $"Boil Noddle {i}");
            }
            await Task.Delay(_msec);
            return new Boil();
        }

        private async Task<Tamago> boilTamago(int _msec, int _debugNum)
        {
            for (int i = 0; i < _debugNum; ++i)
            {
                Debug.Log($"Boil Tamago {i}");
            }
            await Task.Delay(_msec);
            return new Tamago();
        }

        private async Task<Soap> createSoap(int _msec, int _debugNum)
        {
            for (int i = 0; i < _debugNum; ++i)
            {
                Debug.Log($"Create Soap {i}");
            }
            await Task.Delay(_msec);
            return new Soap();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    public class Boil{ }
    public class Tamago { }
    public class Soap { }

}