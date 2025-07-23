using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StatusCounter : MonoBehaviour
{
    public UnityAction OnStatusUp;
    [SerializeField] StatusType statusType;
    [SerializeField] TextMeshProUGUI valText;
    [SerializeField] TextMeshProUGUI diffText;
    [SerializeField] Image updownImage;
    [SerializeField] Button statusUpButton;
    [SerializeField] Sprite upSprite;
    [SerializeField] Sprite downSprite;
    [SerializeField] GameObject statusBar;
    [SerializeField] GameObject bigCell;
    [SerializeField] GameObject middleCell;
    [SerializeField] GameObject smallCell;
    [SerializeField] GameObject singleCell;
    [SerializeField] GameObject cellGroup;
    [SerializeField] int scale = 1; // ステータスのスケール

    private void Start()
    {
        // クリック時の処理を登録
        statusUpButton.onClick.AddListener(OnStatusUpButtonClick);
    }
    private void OnStatusUpButtonClick()
    {
        // スキルポイントが足りない場合は何もしない
        if (PlayerController.Instance.PlayerCharacter.SkillPoint <= 0)
        {
            Debug.LogWarning("スキルポイントが足りません。");
            return;
        }

        // ステータスをアップデート
        PlayerController.Instance.PlayerCharacter.StatusUp(statusType);
        OnStatusUp?.Invoke();
    }
    public void SetCounter(int val, int col)
    {
        valText.text = val.ToString();
        diffText.text = (col - val).ToString();

        if (col > val)
        {
            updownImage.sprite = upSprite;
            updownImage.color = Color.green;
        }
        else if (col < val)
        {
            updownImage.sprite = downSprite;
            updownImage.color = Color.red;
        }
        else
        {
            updownImage.sprite = null;
            updownImage.color = Color.clear;
        }
        SetBar(col);
    }

    public void SetBar(int col)
    {
        // 50でbigCell：１,
        // 10でmiddleCell：1,
        // 5でsmallCell：1,
        // 1でsingleCell：1,(gridで)
        int bigCount = col / (50 * scale);
        int middleCount = (col % (50 * scale)) / (10 * scale);
        int smallCount = (col % (10 * scale)) / (5 * scale);
        int singleCount = col % (5 * scale) / (1 * scale);

        // 既存のセルを削除
        foreach (Transform child in statusBar.transform)
        {
            Destroy(child.gameObject);
        }
        // bigCellを追加
        for (int i = 0; i < bigCount; i++)
        {
            GameObject cell = Instantiate(bigCell, statusBar.transform);
            cell.SetActive(true);
        }
        // middleCellを追加
        for (int i = 0; i < middleCount; i++)
        {
            GameObject cell = Instantiate(middleCell, statusBar.transform);
            cell.SetActive(true);
        }
        // smallCellを追加
        for (int i = 0; i < smallCount; i++)
        {
            GameObject cell = Instantiate(smallCell, statusBar.transform);
            cell.SetActive(true);
        }
        // singleCellを追加
        if (0 < singleCount)
        {
            // cellGroupを追加しその子としてsingleCellを追加していく
            GameObject cellGroupInstance = Instantiate(cellGroup, statusBar.transform);
            foreach (Transform child in cellGroupInstance.transform)
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < singleCount; i++)
            {
                GameObject cell = Instantiate(singleCell, cellGroupInstance.transform);
                cell.SetActive(true);
            }
            cellGroupInstance.SetActive(true);
        }

    }
}
