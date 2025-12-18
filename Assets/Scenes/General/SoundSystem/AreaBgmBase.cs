using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAreaBGM", menuName = "Sound/BGM/Area")]
public class AreaBgmBase : ScriptableObject
{
    [SerializeField] AudioClip bgmClip;
    [SerializeField] AudioClip ambientClip;
    [SerializeField] FieldType fieldType;

    public AudioClip BgmClip() => bgmClip;
    public AudioClip AmbientClip() => ambientClip;
    public FieldType FieldType() => fieldType;
}
