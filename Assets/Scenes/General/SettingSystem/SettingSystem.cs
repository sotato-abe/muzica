using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingSystem : SystemPanel
{
    public static SettingSystem Instance { get; private set; }

    [SerializeField] SettingWindow settingWindow;

    private void Awake()
    {
        PanelClose();
    }

    public override void PanelOpen()
    {
        base.PanelOpen();
        settingWindow.WindowOpen();
    }

    public override void PanelClose()
    {
        base.PanelClose();
        settingWindow.WindowClose();
    }
}
