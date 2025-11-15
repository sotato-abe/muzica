using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingSystem : SystemPanel
{
    public static SettingSystem Instance { get; private set; }

    [SerializeField] SettingWindow settingWindow;

    private void Awake()
    {
        CloseWindow();
    }

    public override void OpenWindow()
    {
        base.OpenWindow();
        settingWindow.PanelOpen();
    }

    public override void CloseWindow()
    {
        base.CloseWindow();
        settingWindow.ClosePanel();
    }
}
