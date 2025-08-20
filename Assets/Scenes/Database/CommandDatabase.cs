using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandDatabase : MonoBehaviour
{
    public static CommandDatabase Instance { get; private set; }
    [SerializeField] List<CommandBase> commandDataList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン切り替えても残す
        }
        else
        {
            Destroy(gameObject); // 重複防止
        }
    }

    public CommandBase LoadCommandData(int commandId)
    {

        if (commandId < 0 || commandId >= commandDataList.Count)
        {
            Debug.LogError("Invalid command ID: " + commandId);
            return null;
        }

        CommandBase command = commandDataList[commandId];
        return command;
    }

    public int GetCommandId(CommandBase command)
    {
        if (command == null)
        {
            return -1;
        }
        Debug.Log($"GetCommandId: {command.Name}");
        return commandDataList.IndexOf(command);
    }
}