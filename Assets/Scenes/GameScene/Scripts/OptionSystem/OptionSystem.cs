using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class OptionSystem : SystemPanel
{
    public static OptionSystem Instance { get; private set; }

    [SerializeField] SelectWindow categorySelectWindow;
    [SerializeField] List<SelectWindow> categoryWindows;

    private void Awake()
    {
        PanelClose();
        categorySelectWindow.OnChangeTarget += ChangeCategory;
        categorySelectWindow.OnEnterTargetWindow += SelectActiveWindow;
        categorySelectWindow.OnExitWindow += PanelClose;
        for (int i = 0; i < categoryWindows.Count; i++)
        {
            categoryWindows[i].OnExitWindow += ExitTargetWindow;
        }
    }

    public override void PanelOpen()
    {
        AgeTimePanel.Instance.SetTimeSpeed(TimeState.Stop);
        PlayerController.Instance.SetFieldPlayerMove(false);
        SoundSystem.Instance.PlaySE(SeType.PanelOpen); // SEをオープンに変更

        base.PanelOpen();
        categorySelectWindow.WindowOpen();
        int selectedIndex = categorySelectWindow.GetCurrentIndex();
        ChangeCategory(selectedIndex);
    }

    public override void PanelClose()
    {
        base.PanelClose();
        categorySelectWindow.WindowClose();
        for (int i = 0; i < categoryWindows.Count; i++)
        {
            categoryWindows[i].WindowClose();
        }
        SoundSystem.Instance.PlaySE(SeType.PanelClose);
        PlayerController.Instance.SetFieldPlayerMove(true);
        AgeTimePanel.Instance.SetTimeSpeed(TimeState.Fast);
    }

    public void ChangeCategory(int index)
    {
        for (int i = 0; i < categoryWindows.Count; i++)
        {
            if (i == index)
            {
                categoryWindows[i].WindowOpen();
            }
            else
            {
                categoryWindows[i].WindowClose();
            }
        }
    }

    public void SelectActiveWindow()
    {
        int selectedIndex = categorySelectWindow.GetCurrentIndex();
        ChangeActiveWindow(false);

        for (int i = 0; i < categoryWindows.Count; i++)
        {
            if (i == selectedIndex)
            {
                categoryWindows[i].ChangeActiveWindow(true);
            }
        }
    }

    public void ExitTargetWindow()
    {
        int selectedIndex = categorySelectWindow.GetCurrentIndex();
        for (int i = 0; i < categoryWindows.Count; i++)
        {
            if (i == selectedIndex)
            {
                categoryWindows[i].ChangeActiveWindow(false);
            }
        }
        ChangeActiveWindow(true);
    }

    public void ChangeActiveWindow(bool isActive)
    {
        categorySelectWindow.ChangeActiveWindow(isActive);
    }
}
