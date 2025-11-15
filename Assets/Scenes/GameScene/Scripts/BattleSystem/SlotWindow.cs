using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlotWindow : Window
{
    [SerializeField] CommandReel commandReel1;
    [SerializeField] CommandReel commandReel2;
    [SerializeField] CommandReel commandReel3;
    [SerializeField] private EquipmentInfo equipmentInfo;

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
        equipmentInfo.CommandUpdate(commandReel1.StopReel());
        yield return new WaitForSeconds(stopInterval);
        equipmentInfo.CommandUpdate(commandReel2.StopReel());
        yield return new WaitForSeconds(stopInterval);
        equipmentInfo.CommandUpdate(commandReel3.StopReel());
    }
}
