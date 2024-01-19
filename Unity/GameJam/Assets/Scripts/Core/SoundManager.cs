using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameJam
{
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager s_instance;
        public static SoundManager Instance => s_instance;

        [SerializeField]
        private GameObject _bgmSorceRoot;

        [SerializeField]
        private GameObject _seSorceRoot;

        private AudioSource _bgmSource = null;
        private readonly List<AudioSource> _seSourceList = new();

        private void Awake()
        {
            s_instance = this;

            _bgmSource = addSorceComponent(_bgmSorceRoot);
            _seSourceList.Add(addSorceComponent(_seSorceRoot));
            _seSourceList.Add(addSorceComponent(_seSorceRoot));
        }

        private static AudioSource addSorceComponent(GameObject rootObj)
        {
            var source = rootObj.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = false;
            return source;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                PlaySeAsync(SoundContainer.Instance.Se1).Forget();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                PlayBgm(SoundContainer.Instance.Bgm, true);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                StopBgm();
            }
        }

        public async UniTask PlaySeAsync(AudioClip audioClip)
        {
            var canPlaySource = _seSourceList.FirstOrDefault(x => x.isPlaying == false);
            if(canPlaySource == null)
            {
                Util.DebugLog("no source");
                canPlaySource = addSorceComponent(_seSorceRoot);
                _seSourceList.Add(canPlaySource);
            }
            canPlaySource.clip = audioClip;
            canPlaySource.Play();
            await UniTask.WaitUntil(() => canPlaySource.isPlaying == false);
            Util.DebugLog("se finish");
        }

        public void PlayBgm(AudioClip audioClip, bool isLoop)
        {
            _bgmSource.clip = audioClip;
            _bgmSource.loop = isLoop;
            _bgmSource.Play();
        }

        public void StopBgm()
        {
            _bgmSource.Stop();
        }
    }

}