using System.Diagnostics;
using UnityEngine;

public class QuestPrefab : MonoBehaviour
{
    [SerializeField] Sprite defaultSprite; // デフォルトのスプライト
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
                if (quest != null && quest.Base != null && QuestController.Instance != null)
                {
                    UnityEngine.Debug.Log($"QuestPrefab: Triggering quest event for Quest Title: {quest.Base.Title}");
                    QuestController.Instance.EnterQuest(quest);
                    isTriggered = true; // イベント発動後、フラグを立てる
                }
                else
                {
                    UnityEngine.Debug.LogError($"QuestPrefab: Null reference detected. Quest: {quest}, Quest.Base: {quest?.Base}, QuestController: {QuestController.Instance}");
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
        ChangeSpriteRenderer(); // スプライトを更新
    }

    private void ChangeSpriteRenderer()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = defaultSprite; // Null チェックを追加
        }
    }
}
