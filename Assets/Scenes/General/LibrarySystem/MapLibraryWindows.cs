using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MapLibraryWindows : SelectWindow
{
    [SerializeField] WorldBigMapWindow worldBigMapWindow;

    public override void WindowOpen()
    {
        base.WindowOpen();
        worldBigMapWindow.WindowOpen();
    }

    public override void WindowClose()
    {
        base.WindowClose();
        worldBigMapWindow.WindowClose();
    }
}
