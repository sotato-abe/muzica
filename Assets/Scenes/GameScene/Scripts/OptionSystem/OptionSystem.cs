using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSystem : MonoBehaviour
{
    [SerializeField] Image backPanel;

    [SerializeField] WorldBigMapWindow worldBigMapWindow;
    [SerializeField] LibraryWindow libraryWindow;
    [SerializeField] SaveWindow saveWindow;
    [SerializeField] SettingWindow settingWindow;
    [SerializeField] AgeTimePanel ageTimePanel;

    private Color blockColor = new Color(0f, 0f, 0f, 200f / 255f);
    private Color inBlockColor = new Color(0f, 0f, 0f, 0f);

    private List<Panel> panelList;

    private void Start()
    {
        panelList = new List<Panel>
        {
            worldBigMapWindow,
            libraryWindow,
            saveWindow,
            settingWindow
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
                    panel.WindowClose();
                    PanelClose();
                    PlayerController.Instance.SetFieldPlayerMove(true);
                    StopTimeState(false);
                    break;
                }
            }
        }
    }
    public void PanelOpen()
    {
        SoundSystem.Instance.PlaySE(SeType.PanelOpen); // SEをオープンに変更
        StartCoroutine(BlockChange(true));
    }

    public void PanelClose()
    {
        SoundSystem.Instance.PlaySE(SeType.PanelClose);
        StartCoroutine(BlockChange(false));
    }

    private IEnumerator BlockChange(bool isOpen)
    {
        float duration = 0.2f;
        float elapsedTime = 0f;
        Color startColor = backPanel.color;
        Color targetColor = isOpen ? blockColor : inBlockColor;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            backPanel.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }
        backPanel.color = targetColor;
        backPanel.raycastTarget = isOpen;
    }

    public void SwitchWorldBigMapWindow()
    {
        SwitchPanel(worldBigMapWindow);
    }

    public void SwitchLibraryWindow()
    {
        SwitchPanel(libraryWindow);
    }

    public void SwitchSaveWindow()
    {
        SwitchPanel(saveWindow);
    }

    public void SwitchSettingWindow()
    {
        SwitchPanel(settingWindow);
    }

    private void SwitchPanel(Panel panel)
    {
        SoundSystem.Instance.PlaySE(SeType.Select);
        foreach (var p in panelList)
        {
            if (p == panel && !p.isActive)
            {
                p.WindowOpen();
                PanelOpen();
                PlayerController.Instance.SetFieldPlayerMove(false);
                StopTimeState(true);
            }
            else if (p == panel && p.isActive)
            {
                p.WindowClose();
                PanelClose();
                PlayerController.Instance.SetFieldPlayerMove(true);
                StopTimeState(false);
            }
            else
            {
                p.WindowClose();
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