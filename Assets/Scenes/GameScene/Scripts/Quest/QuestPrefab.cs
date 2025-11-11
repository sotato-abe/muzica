using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class QuestPrefab : MonoBehaviour
{
    private bool isTriggered = false;
    private float triggerTime = 0f;
    public List<Quest> quests;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isTriggered && other.CompareTag("Player"))
        {
            triggerTime += Time.deltaTime;

            if (triggerTime >= 0.5f)
            {
                // Null チェックを追加
                if (quests != null && quests.Count > 0 && FieldController.Instance != null)
                {
                    FieldController.Instance.EnterQuestBoard(quests);
                    isTriggered = true; // イベント発動後、フラグを立てる
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

    public void SetQuest(List<Quest> newQuests)
    {
        quests = newQuests;
    }
}
