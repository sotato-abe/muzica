using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBGM", menuName = "Sound/BGM")]
public class BgmBase : ScriptableObject
{
    [SerializeField] AudioClip bgmClip;
    [SerializeField] BgmType bgmType;

    public AudioClip BgmClip() => bgmClip;
    public BgmType BgmType() => bgmType;
}
