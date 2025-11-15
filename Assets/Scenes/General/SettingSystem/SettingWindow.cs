using UnityEngine;
using UnityEngine.UI;

public class SettingWindow : Window
{
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;

    private void Start()
    {
        masterSlider.value = SoundSystem.Instance.GetMasterVolume();
        bgmSlider.value = SoundSystem.Instance.GetBGMVolume();
        seSlider.value = SoundSystem.Instance.GetSEVolume();
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
}
