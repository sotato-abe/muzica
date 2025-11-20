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
        categorySelectWindow.OnSelectAction += SelectActiveWindow;
        categorySelectWindow.OnCancelAction += PanelClose;
        for (int i = 0; i < categoryWindows.Count; i++)
        {
            categoryWindows[i].OnCancelAction += CancelActiveWindow;
        }
    }

    public override void PanelOpen()
    {
        characterSelectController.isActive = false;
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

    public void CancelActiveWindow()
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
