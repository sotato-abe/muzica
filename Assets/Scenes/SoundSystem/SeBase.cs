using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSE", menuName = "Sound/SE")]
public class SeBase : ScriptableObject
{
    [SerializeField] AudioClip seClip;
    [SerializeField] SeType seType;

    public AudioClip SeClip() => seClip;
    public SeType SeType() => seType;
}
