using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LibrarySystem : SystemPanel
{
    public static LibrarySystem Instance { get; private set; }

    [SerializeField] SelectWindow categorySelectWindow;
    [SerializeField] List<SelectWindow> categoryWindows;
    [SerializeField] CharacterSelectController characterSelectController;

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
        characterSelectController.isActive = false;
        base.PanelOpen();
        categorySelectWindow.WindowOpen();
        categorySelectWindow.ChangeActiveWindow(true);
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
        characterSelectController.isActive = true;
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
        ChangeActiveWindow(true);
    }

    public void ChangeActiveWindow(bool isActive)
    {
        categorySelectWindow.ChangeActiveWindow(isActive);
    }
}
