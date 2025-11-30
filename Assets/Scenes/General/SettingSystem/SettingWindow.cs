using UnityEngine;
using UnityEngine.UI;

public class SettingWindow : Window
{
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;
    [SerializeField] Slider ambientSlider;

    private void Start()
    {
        masterSlider.value = SoundSystem.Instance.GetMasterVolume();
        bgmSlider.value = SoundSystem.Instance.GetBGMVolume();
        seSlider.value = SoundSystem.Instance.GetSEVolume();
        ambientSlider.value = SoundSystem.Instance.GetAmbientVolume();
    }

    public void OnChangeMasterVolume(float volume)
    {
        SoundSystem.Instance.SetMasterVolume(volume);
    }

    public void OnChangeBGMVolume(float volume)
    {
        SoundSystem.Instance.SetBGMVolume(volume);
    }

    public void OnChangeSEVolume(float volume)
    {
        SoundSystem.Instance.SetSEVolume(volume);
    }
    
    public void OnChangeAmbientVolume(float volume)
    {
        SoundSystem.Instance.SetAmbientVolume(volume);
    }
}
