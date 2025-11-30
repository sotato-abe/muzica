using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour
{
    private void Start()
    {
        SoundSystem.Instance.PlayBGM(BgmType.Opening);
        SoundSystem.Instance.PlayAmbient(AmbientType.Higurashi);
    }
}
