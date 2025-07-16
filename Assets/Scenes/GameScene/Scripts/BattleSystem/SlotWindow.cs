using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlotWindow : Panel
{
    [SerializeField] CommandReel commandReel1;
    [SerializeField] CommandReel commandReel2;
    [SerializeField] CommandReel commandReel3;
    List<Command> resultList;

    public void StopReels(System.Action<List<Command>> onComplete)
    {
        StartCoroutine(GetReelCommands(onComplete));
    }

    private IEnumerator GetReelCommands(System.Action<List<Command>> onComplete)
    {
        resultList = new List<Command>();

        resultList.Add(commandReel1.StopReel());
        yield return new WaitForSeconds(0.3f);

        resultList.Add(commandReel2.StopReel());
        yield return new WaitForSeconds(0.3f);

        resultList.Add(commandReel3.StopReel());

        onComplete?.Invoke(resultList); // 完了時に結果を返す
    }
}
