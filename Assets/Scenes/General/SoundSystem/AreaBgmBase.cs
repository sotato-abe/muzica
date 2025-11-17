using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAreaBGM", menuName = "Sound/BGM/Area")]
public class AreaBgmBase : ScriptableObject
{
    [SerializeField] AudioClip bgmClip;
    [SerializeField] FieldType fieldType;

    public AudioClip BgmClip() => bgmClip;
    public FieldType FieldType() => fieldType;
}
