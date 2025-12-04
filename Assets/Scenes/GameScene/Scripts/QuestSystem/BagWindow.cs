using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class BagWindow : SlidePanel
{
    [SerializeField] CategorySwitch categorySwitch;
    [SerializeField] ItemBoxWindow itemBoxWindow;
    [SerializeField] CommandBoxWindow commandBoxWindow;

    public void Start()
    {
        categorySwitch.OnChangeWindow += ChangeWindow;
        ChangeWindow(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            categorySwitch.SwitchActiveButton();
        }
    }

    public void ChangeWindow(bool isBag)
    {
        itemBoxWindow.gameObject.SetActive(isBag);
        commandBoxWindow.gameObject.SetActive(!isBag);
    }

    public void SetupBagUI()
    {
        itemBoxWindow.SetItems();
        commandBoxWindow.SetCommands();
    }
}
