using UnityEngine;

public class ConfigPanel : Panel
{
    public void SwitchActive()
    {
        isActive = !isActive; // フラグをトグル
        if (isActive)
        {
            PanelOpen();
        }
        else
        {
            ClosePanel();
        }
    }

}