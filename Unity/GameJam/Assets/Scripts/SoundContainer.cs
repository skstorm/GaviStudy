using GameJam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundContainer : MonoBehaviour
{
    private static SoundContainer s_instance;
    public static SoundContainer Instance => s_instance;

    [field: SerializeField]
    public AudioClip Bgm { get; private set; }

    [field: SerializeField]
    public AudioClip Se1 { get; private set; }

    [field: SerializeField]
    public AudioClip Se2 { get; private set; }

    private void Awake()
    {
        s_instance = this;
    }
}
