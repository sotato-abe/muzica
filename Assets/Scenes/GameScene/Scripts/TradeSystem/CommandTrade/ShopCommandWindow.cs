using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ShopCommandWindow : MonoBehaviour, IDropHandler
{
    [SerializeField] CommandBlock commandBlockPrefab;
    [SerializeField] GameObject commandList;

    public delegate void TargetCommandDelegate(Command command, bool isOwn = false);
    public event TargetCommandDelegate OnTargetCommand;

    private const int MAX_BAG_COUNT = 20;
    private Point currentPoint;

    public void OnDrop(PointerEventData eventData)
    {
        // Commandを販売
        CommandBlock droppedCommandBlock = eventData.pointerDrag?.GetComponent<CommandBlock>();

        if (droppedCommandBlock != null && droppedCommandBlock.Command != null)
        {
            Command command = droppedCommandBlock.Command;
            PlayerController.Instance.SellCommand(command);
            droppedCommandBlock.RemoveCommand();
            CreateCommandBlock(command);
        }
        else
        {
            Debug.LogWarning("ドロップされたアイテムが無効です。");
        }
    }

    public void SetPoint(Point point)
    {
        currentPoint = point;
        SetCommands(point.ShopCommands);
    }

    public void SetCommands(List<Command> commands = null)
    {
        DeleteAllCommands();
        foreach (Command command in commands)
        {
            CreateCommandBlock(command);
        }
    }

    private void CreateCommandBlock(Command command)
    {
        CommandBlock commandBlock = Instantiate(commandBlockPrefab, commandList.transform);
        commandBlock.Setup(command, this.transform);
        commandBlock.OnRemoveCommand += SellCommand;
        commandBlock.OnTargetCommand += TargetCommand;
    }

    private void DeleteAllCommands()
    {
        foreach (Transform child in commandList.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private bool SellCommand(CommandBlock commandBlock)
    {
        if (commandBlock == null || commandBlock.Command == null) return false;
        if (commandBlock.OriginalParent != this.transform) return false;

        Command command = commandBlock.Command;
        bool isbuy = PlayerController.Instance.SpendCurrency(command.Base.CoinPrice, command.Base.DiscPrice);        
        if (isbuy)
        {
            commandBlock.RemovePlaceholder();
            Destroy(commandBlock.gameObject);
            currentPoint.ShopCommands.Remove(command); // ポイントのアイテムリストから削除
            return true;
        }
        return false;
    }

    public void TargetCommand(CommandBlock commandBlock)
    {
        if (commandBlock == null || commandBlock.Command == null)
        {
            OnTargetCommand?.Invoke(null);
            return;
        }
        OnTargetCommand?.Invoke(commandBlock.Command);
    }
}
