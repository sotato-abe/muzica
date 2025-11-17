using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public static SoundSystem Instance { get; private set; }

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource seSource;

    [SerializeField] List<BgmBase> bgmClipList;
    [SerializeField] List<AreaBgmBase> areaBgmList;
    [SerializeField] List<SeBase> seClipList;

    private float fadeInTime = 0f;
    private float fadeOutTime = 0.8f;

    private float masterVolume = 1.0f;
    private float bgmVolume = 1.0f;
    private float seVolume = 1.0f;

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

    #region BGM
    public void PlayBGM(BgmType bgmType)
    {
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

    public void SetAreaBGM(FieldType fieldType)
    {
        AreaBgmBase bgmData = areaBgmList.Find(b => b.FieldType() == fieldType);


        if (bgmData != null)
        {
            StartCoroutine(FadeInBGM(bgmData.BgmClip()));
        }
        else
        {
            Debug.LogWarning($"SoundSystem: BGM of type {fieldType} not found.");
        }
    }

    public void SetBGM(AudioClip bgmClip)
    {
        if (bgmClip != null)
        {
            StartCoroutine(FadeInBGM(bgmClip));
        }
        else
        {
            Debug.LogWarning($"SoundSystem: Field BGM data is null.");
        }
    }

    private IEnumerator FadeInBGM(AudioClip newClip)
    {

        if (bgmSource.clip == newClip)
            yield break;
        StopBGM();
        if (bgmSource.isPlaying)
            yield return StartCoroutine(FadeOutBGM());
        bgmSource.clip = newClip;
        bgmSource.volume = 0;
        bgmSource.Play();

        for (float t = 0; t < fadeInTime; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(0, 1, t / fadeInTime);
            yield return null;
        }
        bgmSource.volume = 1;
    }

    public void StopBGM()
    {
        if (!bgmSource.isPlaying) return;
        StartCoroutine(FadeOutBGM());
    }
    #endregion

    private IEnumerator FadeOutBGM()
    {
        float startVolume = bgmSource.volume;
        for (float t = 0; t < fadeOutTime; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(startVolume, 0, t / fadeOutTime);
            yield return null;
        }
        bgmSource.Stop();
        bgmSource.volume = startVolume;
    }

    #region SE
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
    #endregion

    #region Volume Control
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        bgmSource.volume = bgmVolume * masterVolume;
        seSource.volume = seVolume * masterVolume;
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        bgmSource.volume = bgmVolume * masterVolume;
    }

    public float GetBGMVolume()
    {
        return bgmVolume;
    }

    public void SetSEVolume(float volume)
    {
        seVolume = volume;
        seSource.volume = seVolume * masterVolume;
    }

    public float GetSEVolume()
    {
        return seVolume;
    }
    #endregion
}
