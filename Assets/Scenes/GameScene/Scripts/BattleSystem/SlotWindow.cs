using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlotWindow : Window
{
    [SerializeField] CommandReel commandReel1;
    [SerializeField] CommandReel commandReel2;
    [SerializeField] CommandReel commandReel3;
    [SerializeField] private EquipmentCard equipmentCard;

    private float startInterval = 0.3f; // リール開始の遅延時間
    private float stopInterval = 0.8f; // リール停止の遅延時間
    List<Command> resultList;

    public IEnumerator StartSlot()
    {
        commandReel1.StartReel();
        yield return new WaitForSeconds(startInterval);
        commandReel2.StartReel();
        yield return new WaitForSeconds(startInterval);
        commandReel3.StartReel();
    }

    public IEnumerator StopSlot()
    {
        yield return StartCoroutine(commandReel1.StopReel());
        equipmentCard.CommandUpdate(commandReel1.GetActiveCommand());
        yield return new WaitForSeconds(stopInterval);
        yield return StartCoroutine(commandReel2.StopReel());
        equipmentCard.CommandUpdate(commandReel2.GetActiveCommand());
        yield return new WaitForSeconds(stopInterval);
        yield return StartCoroutine(commandReel3.StopReel());
        equipmentCard.CommandUpdate(commandReel3.GetActiveCommand());
    }
}