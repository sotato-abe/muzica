using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingSystem : SystemPanel
{
    public static SettingSystem Instance { get; private set; }

    [SerializeField] SettingWindow settingWindow;

    private bool isActive = false;

    private void Awake()
    {
        PanelClose();
    }

    
    private void Update()
    {
        if(!isActive) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PanelClose();
        }
    }

    public override void PanelOpen()
    {
        base.PanelOpen();
        settingWindow.WindowOpen();
        isActive = true;
    }

    public override void PanelClose()
    {
        base.PanelClose();
        settingWindow.WindowClose();
        isActive = false;
    }
}
