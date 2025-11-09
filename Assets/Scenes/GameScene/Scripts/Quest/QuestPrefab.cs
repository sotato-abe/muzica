using System.Diagnostics;
using UnityEngine;

public class QuestPrefab : MonoBehaviour
{
    private bool isTriggered = false;
    private float triggerTime = 0f;
    public Quest quest;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isTriggered && other.CompareTag("Player"))
        {
            triggerTime += Time.deltaTime;

            if (triggerTime >= 0.5f)
            {
                // Null チェックを追加
                if (quest != null && quest.Base != null && FieldController.Instance != null)
                {
                    UnityEngine.Debug.Log("QuestPrefab: Triggering quest event for Quest Title: " + quest.Base.Title);
                    FieldController.Instance.EnterQuest(quest);
                    isTriggered = true; // イベント発動後、フラグを立てる
                }
                else
                {
                    UnityEngine.Debug.LogError($"QuestPrefab: Null reference detected. Quest: {quest}, Quest.Base: {quest?.Base}");
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            triggerTime = 0f;
            isTriggered = false;
        }
    }

    public void SetQuest(Quest newQuest)
    {
        quest = newQuest;
    }
}
