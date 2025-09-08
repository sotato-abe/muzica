using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionGroup : MonoBehaviour
{
    [SerializeField] WorldBigMapPanel worldBigMapPanel;
    [SerializeField] LibraryPanel libraryPanel;
    [SerializeField] SavePanel savePanel;
    [SerializeField] ConfigPanel configPanel;
    [SerializeField] AgeTimePanel ageTimePanel;

    private List<Panel> panelList;

    private void Start()
    {
        panelList = new List<Panel>
        {
            worldBigMapPanel,
            libraryPanel,
            savePanel,
            configPanel
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            foreach (var panel in panelList)
            {
                if (panel.isActive)
                {
                    panel.ClosePanel();
                    PlayerController.Instance.SetFieldPlayerMove(true);
                    StopTimeState(false);
                    break;
                }
            }
        }
    }

    public void SwitchWorldBigMapPanel()
    {
        SwitchPanel(worldBigMapPanel);
    }

    public void SwitchLibraryPanel()
    {
        SwitchPanel(libraryPanel);
    }

    public void SwitchSavePanel()
    {
        SwitchPanel(savePanel);
    }

    public void SwitchConfigPanel()
    {
        SwitchPanel(configPanel);
    }

    private void SwitchPanel(Panel panel)
    {
        foreach (var p in panelList)
        {
            if (p == panel && !p.isActive)
            {
                p.PanelOpen();
                PlayerController.Instance.SetFieldPlayerMove(false);
                StopTimeState(true);
            }
            else if (p == panel && p.isActive)
            {
                p.ClosePanel();
                PlayerController.Instance.SetFieldPlayerMove(true);
                StopTimeState(false);
            }
            else
            {
                StopTimeState(false);
                p.ClosePanel();
            }
        }
    }

    private void StopTimeState(bool isActive)
    {
        if (isActive)
        {
            ageTimePanel.SetTimeSpeed(TimeState.Stop);
        }
        else
        {
            ageTimePanel.SetTimeSpeed(TimeState.Fast);
        }
    }
}