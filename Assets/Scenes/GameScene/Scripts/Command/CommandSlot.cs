using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class CommandSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] CommandBlock commandBlockPrefab;

    public int SlotIndex;

    public void OnDrop(PointerEventData eventData)
    {
        // ドロップアイテムをバックに追加
        CommandBlock droppedCommandBlock = eventData.pointerDrag?.GetComponent<CommandBlock>();

        if (droppedCommandBlock == null || droppedCommandBlock.Command == null)
        {
            Debug.LogWarning("ドロップされたアイテムが無効です。");
            return;
        }
        if (droppedCommandBlock.OriginalParent == this.transform)
            return;

        Command command = droppedCommandBlock.Command;
        PlayerController.Instance.AddCommandToSlot(command, SlotIndex);
        SetCommand(command);
        SoundSystem.Instance.PlaySE(SeType.Set);
        droppedCommandBlock.RemoveCommand();
    }

    public void SetCommand(Command command)
    {
        CommandBlock newBlock = Instantiate(commandBlockPrefab, this.transform);
        newBlock.OnRemoveCommand += RemoveCommand;
        newBlock.Setup(command, this.transform);
    }

    public void ResetSlot()
    {
        foreach (Transform child in transform)
        {
            CommandBlock childBlock = child.GetComponent<CommandBlock>();
            if (childBlock != null)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private bool RemoveCommand(CommandBlock commandBlock)
    {
        if (commandBlock == null || commandBlock.Command == null) return false;
        if (commandBlock.OriginalParent != this.transform) return false;

        Command command = commandBlock.Command;
        PlayerController.Instance.RemoveCommandFromSlot(SlotIndex);
        commandBlock.RemovePlaceholder();
        Destroy(commandBlock.gameObject);
        return true;
    }
}
