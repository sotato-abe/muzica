using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAmbient", menuName = "Sound/Ambient")]
public class AmbientBase : ScriptableObject
{
    [SerializeField] AudioClip ambientClip;
    [SerializeField] AmbientType ambientType;

    public AudioClip AmbientClip() => ambientClip;
    public AmbientType AmbientType() => ambientType;
}
