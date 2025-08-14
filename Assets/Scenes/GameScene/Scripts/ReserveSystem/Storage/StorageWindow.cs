using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StorageWindow : MonoBehaviour, IDropHandler
{
    [SerializeField] CommandBlock commandBlockPrefab;
    [SerializeField] GameObject refusalBlockPrefab;
    [SerializeField] GameObject commandList;
    [SerializeField] GameObject blockList;
    [SerializeField] TextMeshProUGUI counterText;
    [SerializeField] int maxCount = 15;
    Dictionary<Command, CommandBlock> commandBlockMap = new Dictionary<Command, CommandBlock>();

    public delegate void TargetCommandDelegate(Command command, bool isOwn = true);
    public event TargetCommandDelegate OnTargetCommand;
    private void Awake()
    {
        DeleteAllCommands();
    }
    private void OnEnable()
    {
        Debug.Log("StorageWindow OnEnable");
        SetCommands();
        SetBlock();
    }

    public void OnDrop(PointerEventData eventData)
    {
        // ドロップアイテムをバックに追加
        CommandBlock droppedCommandBlock = eventData.pointerDrag?.GetComponent<CommandBlock>();
        if (droppedCommandBlock == null || droppedCommandBlock.Command == null) return;
        if (droppedCommandBlock.OriginalParent == this.transform) return;

        Command command = droppedCommandBlock.Command;
        bool canBuy = droppedCommandBlock.RemoveCommand();

        if (canBuy)
        {
            PlayerController.Instance.AddCommandToStorage(droppedCommandBlock.Command);
            SetCommands();
        }
    }

    public void SetCommands()
    {
        List<Command> commands = PlayerController.Instance.PlayerCharacter.StorageList;
        var commandsToRemove = new List<Command>();

        foreach (var kvp in commandBlockMap)
        {
            if (!commands.Contains(kvp.Key))
            {
                commandsToRemove.Add(kvp.Key);
            }
        }

        foreach (var command in commandsToRemove)
        {
            if (commandBlockMap.TryGetValue(command, out CommandBlock commandBlock))
            {
                Destroy(commandBlock.gameObject);
                commandBlockMap.Remove(command);
            }
        }

        foreach (Command command in commands)
        {
            if (commandBlockMap.ContainsKey(command))
            {
                continue;
            }

            CreateCommandBlock(command);
        }
    }

    private void CreateCommandBlock(Command command)
    {
        if (commandBlockMap.ContainsKey(command))
        {
            // 既に表示済みならスキップ
            return;
        }

        CommandBlock commandBlock = Instantiate(commandBlockPrefab, commandList.transform);
        commandBlock.Setup(command, this.transform);
        commandBlock.OnRemoveCommand += RemoveCommand;
        commandBlock.OnTargetCommand += TargetCommand;
        commandBlockMap[command] = commandBlock;
    }

    private void SetBlock()
    {
        int newBlockCount = maxCount - PlayerController.Instance.PlayerCharacter.ColStorage;
        foreach (Transform child in blockList.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < newBlockCount; i++)
        {
            Instantiate(refusalBlockPrefab, blockList.transform);
        }
    }

    private void DeleteAllCommands()
    {
        foreach (Transform child in commandList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var commandBlock in commandBlockMap.Values)
        {
            Destroy(commandBlock.gameObject);
        }
        commandBlockMap.Clear();
    }

    private bool RemoveCommand(CommandBlock commandBlock)
    {
        if (commandBlock == null || commandBlock.Command == null) return false;
        if (commandBlock.OriginalParent != this.transform) return false;

        Command command = commandBlock.Command;
        PlayerController.Instance.RemoveCommandFromStorage(command);
        commandBlock.RemovePlaceholder();
        commandBlockMap.Remove(command);
        Destroy(commandBlock.gameObject);
        SetCommands();
        return true;
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
