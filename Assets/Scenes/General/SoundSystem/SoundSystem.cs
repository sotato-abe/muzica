using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public static SoundSystem Instance { get; private set; }

    // PlayerPrefsキー定数
    private const string MASTER_VOLUME_KEY = "MasterVolume";
    private const string BGM_VOLUME_KEY = "BGMVolume";
    private const string SE_VOLUME_KEY = "SEVolume";
    private const string AMBIENT_VOLUME_KEY = "AmbientVolume";

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource seSource;
    [SerializeField] private AudioSource ambientSource;

    [SerializeField] List<BgmBase> bgmClipList;
    [SerializeField] List<AreaBgmBase> areaBgmList;
    [SerializeField] List<SeBase> seClipList;
    [SerializeField] List<AmbientBase> ambientClipList;

    private float fadeInTime = 0f;
    private float fadeOutTime = 0.8f;

    private float masterVolume = 1.0f;
    private float bgmVolume = 1.0f;
    private float seVolume = 1.0f;
    private float ambientVolume = 0.7f;

    private void Awake()
    {
        UnityEngine.Debug.Log("SoundSystem Awake");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン切り替えても残す
            
            // 保存されたボリューム設定を読み込み
            LoadVolumeSettings();
        }
        else
        {
            Destroy(gameObject); // 重複防止
        }
    }

    private void Start()
    {
        // シーン切り替え後にAudioSourceが再アタッチされた場合の対応
        ApplyVolumeSettings();
    }

    private void LoadVolumeSettings()
    {
        masterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 1.0f);
        bgmVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 1.0f);
        seVolume = PlayerPrefs.GetFloat(SE_VOLUME_KEY, 1.0f);
        ambientVolume = PlayerPrefs.GetFloat(AMBIENT_VOLUME_KEY, 0.7f);
        
        // AudioSourceにボリュームを適用
        ApplyVolumeSettings();
    }

    private void ApplyVolumeSettings()
    {
        if (bgmSource != null) bgmSource.volume = bgmVolume * masterVolume;
        if (seSource != null) seSource.volume = seVolume * masterVolume;
        if (ambientSource != null) ambientSource.volume = ambientVolume * masterVolume;
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

        float targetVolume = bgmVolume * masterVolume;
        for (float t = 0; t < fadeInTime; t += Time.deltaTime)
        {
            bgmSource.volume = Mathf.Lerp(0, targetVolume, t / fadeInTime);
            yield return null;
        }
        bgmSource.volume = targetVolume;
    }

    public void StopBGM()
    {
        if (!bgmSource.isPlaying) return;
        StartCoroutine(FadeOutBGM());
    }

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
    #endregion

    #region Ambient
    public void PlayAmbient(AmbientType ambientType)
    {
        AmbientBase ambientData = ambientClipList.Find(a => a.AmbientType() == ambientType);
        if (ambientData != null)
        {
            StartCoroutine(FadeInAmbient(ambientData.AmbientClip()));
        }
        else
        {
            Debug.LogWarning($"SoundSystem: Ambient of type {ambientType} not found.");
        }
    }

    public void SetAmbientBGM(AmbientType ambientType)
    {
        AmbientBase ambientData = ambientClipList.Find(a => a.AmbientType() == ambientType);

        if (ambientData != null)
        {
            ambientSource.clip = ambientData.AmbientClip();
            ambientSource.volume = ambientVolume * masterVolume;
            ambientSource.loop = true;
            ambientSource.Play();
        }
        else
        {
            Debug.LogWarning($"SoundSystem: Ambient of type {ambientType} not found.");
        }
    }

    private IEnumerator FadeInAmbient(AudioClip newClip)
    {

        if (ambientSource.clip == newClip)
            yield break;
        StopAmbient();
        if (ambientSource.isPlaying)
            yield return StartCoroutine(FadeOutAmbient());
        ambientSource.clip = newClip;
        ambientSource.volume = 0;
        ambientSource.Play();

        float targetVolume = ambientVolume * masterVolume;
        for (float t = 0; t < fadeInTime; t += Time.deltaTime)
        {
            ambientSource.volume = Mathf.Lerp(0, targetVolume, t / fadeInTime);
            yield return null;
        }
        ambientSource.volume = targetVolume;
    }

    public void StopAmbient()
    {
        if (!ambientSource.isPlaying) return;
        StartCoroutine(FadeOutAmbient());
    }

    private IEnumerator FadeOutAmbient()
    {
        float startVolume = ambientSource.volume;
        for (float t = 0; t < fadeOutTime; t += Time.deltaTime)
        {
            ambientSource.volume = Mathf.Lerp(startVolume, 0, t / fadeOutTime);
            yield return null;
        }
        ambientSource.Stop();
        ambientSource.volume = startVolume;
    }
    #endregion

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
        ambientSource.volume = ambientVolume * masterVolume;
        
        // PlayerPrefsに保存
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, masterVolume);
        PlayerPrefs.Save();
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }

    public void SetBGMVolume(float volume)
    {
        UnityEngine.Debug.Log("SetBGMVolume: " + volume);
        bgmVolume = volume;
        bgmSource.volume = bgmVolume * masterVolume;
        
        // PlayerPrefsに保存
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, bgmVolume);
        PlayerPrefs.Save();
    }

    public float GetBGMVolume()
    {
        UnityEngine.Debug.Log("GetBGMVolume: " + bgmVolume);
        return bgmVolume;
    }

    public void SetSEVolume(float volume)
    {
        seVolume = volume;
        seSource.volume = seVolume * masterVolume;
        
        // PlayerPrefsに保存
        PlayerPrefs.SetFloat(SE_VOLUME_KEY, seVolume);
        PlayerPrefs.Save();
    }

    public float GetSEVolume()
    {
        return seVolume;
    }

    public void SetAmbientVolume(float volume)
    {
        ambientVolume = volume;
        ambientSource.volume = ambientVolume * masterVolume;
        
        // PlayerPrefsに保存
        PlayerPrefs.SetFloat(AMBIENT_VOLUME_KEY, ambientVolume);
        PlayerPrefs.Save();
    }
    public float GetAmbientVolume()
    {
        return ambientVolume;
    }
    #endregion
}
