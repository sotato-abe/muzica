using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveDataButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject saveDisc;
    [SerializeField] private TextMeshProUGUI emptyText;
    [SerializeField] private Image characterImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI eventText;
    [SerializeField] int saveIndex;

    public delegate void SaveGameDelegate(int index);
    public SaveGameDelegate OnSaveGame;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    public void Setup(SimpleData saveData)
    {
        if (saveData == null)
        {
            SetEmpty();
            return;
        }

        emptyText.gameObject.SetActive(false);
        saveDisc.SetActive(true);

        characterImage.sprite = saveData.characterSprite;
        nameText.text = saveData.name;
        levelText.text = "Lv." + saveData.level.ToString();
        timeText.text = saveData.time.ToString("yyyy/MM/dd") + " " + saveData.position;
        eventText.text = "test";
    }

    public void SetEmpty()
    {
        emptyText.gameObject.SetActive(true);
        saveDisc.SetActive(false);
    }

    private void OnButtonClick()
    {
        OnSaveGame?.Invoke(saveIndex);
    }
}