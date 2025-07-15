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
    PlayerController playerController;
    Dictionary<Command, CommandBlock> commandBlockMap = new Dictionary<Command, CommandBlock>();

    public delegate void TargetCommandDelegate(Command? command);
    public event TargetCommandDelegate OnTargetCommand;

    private const int MAX_STORAGE_COUNT = 20;
    private int currentBlockCount = 0;
    private void Awake()
    {
        playerController = PlayerController.Instance;
        DeleteAllCommands();
    }
    private void OnEnable()
    {
        Debug.Log("test1");
        if (PlayerController.Instance == null) return;
        Debug.Log("test2");

        playerController = PlayerController.Instance;
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
        playerController.AddCommandToStorage(droppedCommandBlock.Command);
        droppedCommandBlock.RemoveCommand();
        CreateCommandBlock(command, null);
        SetCounter();
    }

    public void SetCommands()
    {
        List<Command> commands = playerController.PlayerCharacter.StorageList;

        foreach (Command command in commands)
        {
            if (commandBlockMap.ContainsKey(command))
            {
                // 既に表示済みならスキップ
                continue;
            }

            // 新規アイテムだけ生成
            CreateCommandBlock(command, "New");
        }
        SetCounter();
    }

    private void CreateCommandBlock(Command command, string? statusText)
    {
        if (commandBlockMap.ContainsKey(command))
        {
            // 既に表示済みならスキップ
            return;
        }

        CommandBlock commandBlock = Instantiate(commandBlockPrefab, commandList.transform);
        commandBlock.Setup(command, this.transform);
        commandBlock.SetStatustext(statusText);
        commandBlock.OnRemoveCommand += RemoveCommand;
        commandBlock.OnTargetCommand += TargetCommand;
        commandBlockMap[command] = commandBlock;
        Debug.Log($"CommandBlock created for command: {command.Base.Name}");
    }

    private void SetCounter()
    {
        counterText.text = $"{commandBlockMap.Count} / {playerController.PlayerCharacter.ColStorage}";
    }

    private void SetBlock()
    {
        int newBlockCount = MAX_STORAGE_COUNT - playerController.PlayerCharacter.ColStorage;
        if (currentBlockCount == newBlockCount)
            return;

        currentBlockCount = newBlockCount;
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
        SetCounter();
    }

    private bool RemoveCommand(CommandBlock commandBlock)
    {
        if (commandBlock == null || commandBlock.Command == null) return false;
        if (commandBlock.OriginalParent != this.transform) return false;

        Command command = commandBlock.Command;
        playerController.RemoveCommandFromStorage(commandBlock.Command);
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
