using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadDataButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject loadDisc;
    [SerializeField] private Image characterImage;
    [SerializeField] private TextMeshProUGUI emptyText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI eventText;
    [SerializeField] int saveIndex;

    public delegate void LoadGameDelegate(int index);
    public LoadGameDelegate OnStartGame;

    bool isLoaded = false;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
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

        nameText.text = loadData.name;
        timeText.text = loadData.time.ToString() + " " + loadData.position;
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
        if (isLoaded)
        {
            OnStartGame?.Invoke(saveIndex);
        }
    }
}