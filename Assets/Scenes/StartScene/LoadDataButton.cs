using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LoadDataButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject loadDisc;
    [SerializeField] private TextMeshProUGUI emptyText;
    [SerializeField] private Image characterImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI eventText;
    [SerializeField] GameObject loadIcon;
    [SerializeField] int saveIndex;

    public delegate void LoadGameDelegate(int index);
    public LoadGameDelegate OnStartGame;

    bool isLoaded = false;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
        loadIcon.SetActive(false);
    }

    public void Setup(SimpleData loadData)
    {
        if (loadData == null)
        {
            SetEmpty();
            return;
        }

        isLoaded = true;
        emptyText.gameObject.SetActive(false);
        loadDisc.SetActive(true);

        characterImage.sprite = loadData.characterSprite;
        nameText.text = loadData.name;
        levelText.text = "Lv." + loadData.level.ToString();
        timeText.text = loadData.time.ToString("yyyy/MM/dd") + " " + loadData.position;
        eventText.text = "test";
    }

    public void SetEmpty()
    {
        isLoaded = false;
        emptyText.gameObject.SetActive(true);
        loadDisc.SetActive(false);
    }

    private void OnButtonClick()
    {
        SoundSystem.Instance.PlaySE(SeType.Select);
        if (isLoaded)
        {
            OnStartGame?.Invoke(saveIndex);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundSystem.Instance.PlaySE(SeType.Cursol);
        loadIcon.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        loadIcon.SetActive(false);
    }
}