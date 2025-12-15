using System.Collections;
using UnityEngine;

public class SystemController : MonoBehaviour
{
    [SerializeField] SystemPanel librarySystemPanel;
    [SerializeField] SystemPanel settingSystemPanel;

    #region 設定構造体
    #endregion

    public void OpenLibrarySystem()
    {
        settingSystemPanel.PanelClose();
        librarySystemPanel.PanelOpen();
    }

    public void CloseLibrarySystem()
    {
        librarySystemPanel.PanelClose();
    }

    public void OpenSettingSystem()
    {
        librarySystemPanel.PanelClose();
        settingSystemPanel.PanelOpen();
    }

    public void CloseSettingSystem()
    {
        settingSystemPanel.PanelClose();
    }
}