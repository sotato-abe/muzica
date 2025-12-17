using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CommandLibraryWindows : SelectWindow
{
    [SerializeField] private GameObject contentArea;
    [SerializeField] private SelectElement selectElementObject;
    [SerializeField] CommandCard commandCard;

    private List<CommandBase> commandBaseList;

    public override void Start()
    {
        SetListElement();
        ChangeActiveWindow(false);
    }

    public override void WindowOpen()
    {
        base.WindowOpen();
    }

    public override void WindowClose()
    {
        base.WindowClose();
    }

    public override void EnterTargetWindow()
    {
        // 選択ウィンドウの末端なのでターゲット移動を止める
    }

    private void SetListElement()
    {
        foreach (Transform child in contentArea.transform)
        {
            Destroy(child.gameObject);
        }
        commandBaseList = CommandDatabase.Instance.GetAllCommandBases();
        for (int i = 0; i < commandBaseList.Count; i++)
        {
            CommandBase commandBase = commandBaseList[i];
            SelectElement newElement = Instantiate(selectElementObject, contentArea.transform);
            newElement.SetElementText(commandBase.Name);
            newElement.SetIndex(i);
            newElement.SetActiveCursol(false);
            selectElements.Add(newElement);
        }

        int initialIndex = GetCurrentIndex();
        selectElements[initialIndex].SetActiveCursol(true);
        TargetElement(initialIndex);
    }

    public override void TargetElement(int index)
    {
        CommandBase commandBase = commandBaseList[index];
        Command targetCommand = new Command(commandBase);
        commandCard.SetCommand(targetCommand);
    }
}
