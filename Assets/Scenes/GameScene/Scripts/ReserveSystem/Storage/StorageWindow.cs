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
        if (PlayerController.Instance == null) return;

        // playerController = PlayerController.Instance;
        SetCommands();
        SetBlock();
    }

    public void OnDrop(PointerEventData eventData)
    {
        // ドロップアイテムをバックに追加
        CommandBlock droppedCommandBlock = eventData.pointerDrag?.GetComponent<CommandBlock>();

        if (droppedCommandBlock != null && droppedCommandBlock.Command != null)
        {
            Command command = droppedCommandBlock.Command;
            if (droppedCommandBlock.OriginalParent == this.transform)
                return;
            playerController.AddCommandToStorage(droppedCommandBlock.Command);
            droppedCommandBlock.RemoveCommand();
            SetCommands();
        }
        else
        {
            Debug.LogWarning("ドロップされたアイテムが無効です。");
        }
    }

    public void SetCommands()
    {
        List<Command> commands = playerController.PlayerCharacter.StorageList;
        Debug.Log($"SetCommands: {commands.Count} commands in storage.");

        foreach (Command command in commands)
        {
            if (commandBlockMap.ContainsKey(command))
            {
                // 既に表示済みならスキップ
                continue;
            }

            // 新規アイテムだけ生成
            CommandBlock commandBlock = Instantiate(commandBlockPrefab, commandList.transform);
            commandBlock.Setup(command, this.transform);
            commandBlock.SetStatustext("New");
            commandBlock.OnRemoveCommand += RemoveCommand;
            commandBlock.OnTargetCommand += TargetCommand;
            commandBlockMap[command] = commandBlock;
        }

        SetCounter();
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

    private void RemoveCommand(CommandBlock commandBlock)
    {
        if (commandBlock == null || commandBlock.Command == null) return;

        Command command = commandBlock.Command;
        if (commandBlockMap.ContainsKey(command))
        {
            playerController.RemoveCommandFromStorage(commandBlock.Command);
            commandBlock.RemovePlaceholder();
            commandBlockMap.Remove(command);
            Destroy(commandBlock.gameObject);
            SetCommands();   
        }
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
