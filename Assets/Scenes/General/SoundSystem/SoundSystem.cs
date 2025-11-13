using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public static SoundSystem Instance { get; private set; }

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource seSource;

    [SerializeField] List<BgmBase> bgmClipList;
    [SerializeField] List<SeBase> seClipList;

    private float fadeTime = 0.8f;

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
        StopBGM();
        BgmBase bgmData = bgmClipList.Find(b => b.BgmType() == bgmType);
        if (bgmData != null)
        {
            StartCoroutine(FadeInBGM(bgmData.BgmClip()));
        }
        else
        {
            Debug.LogWarning($"SoundSystem: BGM of type {bgmType} not found.");
        }
    }

    private IEnumerator FadeInBGM(AudioClip newClip)
    {
        if (bgmSource.isPlaying)
            yield return StartCoroutine(FadeOutBGM());
        bgmSource.clip = newClip;
        bgmSource.volume = 0;
        bgmSource.Play();

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(0, 1, t / fadeTime);
            yield return null;
        }
        bgmSource.volume = 1;
    }

    public void StopBGM()
    {
        if (!bgmSource.isPlaying) return;
        StartCoroutine(FadeOutBGM());
    }

    private IEnumerator FadeOutBGM()
    {
        float startVolume = bgmSource.volume;
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0, t / fadeTime);
            yield return null;
        }
        bgmSource.Stop();
        bgmSource.volume = startVolume;
    }

    public void PlaySE(SeType seType)
    {
        SeBase seData = seClipList.Find(s => s.SeType() == seType);
        if (seData != null)
        {
            seSource.PlayOneShot(seData.SeClip());
        }
        else
        {
            Debug.LogWarning($"SoundSystem: SE of type {seType} not found.");
        }
    }
}
