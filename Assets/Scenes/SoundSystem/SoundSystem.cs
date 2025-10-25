using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public static SoundSystem Instance { get; private set; }

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource seSource;

    [SerializeField] List<BgmBase> bgmClipList;
    [SerializeField] List<AudioClip> seClipList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン切り替えても残す

        }
        else
        {
            Destroy(gameObject); // 重複防止
        }
    }
    
    public void PlayBGM(BgmType bgmType)
    {
        BgmBase bgmData = bgmClipList.Find(b => b.BgmType() == bgmType);
        if (bgmData != null)
        {
            bgmSource.clip = bgmData.BgmClip();
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"SoundSystem: BGM of type {bgmType} not found.");
        }
    }
}
