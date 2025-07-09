using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

// ItemBlockをドロップするためのドロップボックス
// TODO：commandBlockをドロップする
public class DropBox : FocusScaleUp, IDropHandler
{
    public virtual void OnDrop(PointerEventData eventData)
    {
        ItemBlock itemBlock = eventData.pointerDrag?.GetComponent<ItemBlock>();
        if (itemBlock != null || itemBlock?.Item != null)
        {
            PlayerController.Instance.DropItem(itemBlock.Item);
            itemBlock.RemoveItem();
            return;
        }

        CommandBlock commandBlock = eventData.pointerDrag?.GetComponent<CommandBlock>();
        if (commandBlock != null || commandBlock?.Command != null)
        {
            PlayerController.Instance.DropCommand(commandBlock.Command);
            commandBlock.RemoveCommand();
            return;
        }
    }
}
