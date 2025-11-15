using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LibrarySystem : SystemPanel
{
    public static LibrarySystem Instance { get; private set; }

    [SerializeField] SelectWindow categorySelectWindow;
    [SerializeField] List<SelectWindow> categoryWindows;

    private void Awake()
    {
        CloseWindow();
        categorySelectWindow.OnChangeTarget += ChangeCategory;
        categorySelectWindow.OnSelectAction += SelectActiveWindow;
        for (int i = 0; i < categoryWindows.Count; i++)
        {
            categoryWindows[i].OnCancelAction += CancelActiveWindow;
        }
    }

    public override void OpenWindow()
    {
        base.OpenWindow();
        categorySelectWindow.PanelOpen();
        int selectedIndex = categorySelectWindow.GetCurrentIndex();
        ChangeCategory(selectedIndex);
    }

    public override void CloseWindow()
    {
        base.CloseWindow();
        categorySelectWindow.ClosePanel();
        for (int i = 0; i < categoryWindows.Count; i++)
        {
            categoryWindows[i].ClosePanel();
        }
    }

    public void ChangeCategory(int index)
    {
        for (int i = 0; i < categoryWindows.Count; i++)
        {
            if (i == index)
            {
                categoryWindows[i].PanelOpen();
            }
            else
            {
                categoryWindows[i].ClosePanel();
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
