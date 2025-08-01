using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlotWindow : Panel
{
    [SerializeField] CommandReel commandReel1;
    [SerializeField] CommandReel commandReel2;
    [SerializeField] CommandReel commandReel3;

    private float interval = 0.3f; // リール開始の遅延時間
    List<Command> resultList;

    public IEnumerator StartReels()
    {
        commandReel1.StartReel();
        yield return new WaitForSeconds(interval);
        commandReel2.StartReel();
        yield return new WaitForSeconds(interval);
        commandReel3.StartReel();
    }

    public void StopReels(System.Action<List<Command>> onComplete)
    {
        StartCoroutine(GetReelCommands(onComplete));
    }

    private IEnumerator GetReelCommands(System.Action<List<Command>> onComplete)
    {
        resultList = new List<Command>();

        resultList.Add(commandReel1.StopReel());
        yield return new WaitForSeconds(interval);

        resultList.Add(commandReel2.StopReel());
        yield return new WaitForSeconds(interval);

        resultList.Add(commandReel3.StopReel());

        onComplete?.Invoke(resultList); // 完了時に結果を返す
    }
}
