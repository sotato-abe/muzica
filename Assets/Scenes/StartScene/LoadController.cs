using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class LoadController : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] LoadDataButton loadDataButton1;
    [SerializeField] LoadDataButton loadDataButton2;
    [SerializeField] LoadDataButton loadDataButton3;

    public const string FILE_SOLA = "solaData.json";
    public const string FILE_NAME1 = "playData1.json";
    public const string FILE_NAME2 = "playData2.json";
    public const string FILE_NAME3 = "playData3.json";

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        LoadData();
        loadDataButton1.OnStartGame += LoadGame;
        loadDataButton2.OnStartGame += LoadGame;
        loadDataButton3.OnStartGame += LoadGame;
    }

    private void LoadData()
    {
        PlayData playData1 = Persistance.Load<PlayData>(FILE_NAME1);
        PlayData playData2 = Persistance.Load<PlayData>(FILE_NAME2);
        PlayData playData3 = Persistance.Load<PlayData>(FILE_NAME3);

        if (playData1 != null)
        {
            SimpleData simpleData = new SimpleData(playData1);
            loadDataButton1.Setup(simpleData);
        }
        if (playData2 != null)
        {
            SimpleData simpleData = new SimpleData(playData2);
            loadDataButton2.Setup(simpleData);
        }
        if (playData3 != null)
        {
            SimpleData simpleData = new SimpleData(playData3);
            loadDataButton3.Setup(simpleData);
        }
    }

    void StartGame()
    {

        PlayData selectedPlayData = Persistance.Load<PlayData>(FILE_SOLA);
        GameScene.selectedPlayData = selectedPlayData;
        SceneManager.LoadScene("GameScene");
    }
    
    void LoadGame(int index)
    {
        PlayData selectedPlayData = index switch
        {
            1 => Persistance.Load<PlayData>(FILE_NAME1),
            2 => Persistance.Load<PlayData>(FILE_NAME2),
            3 => Persistance.Load<PlayData>(FILE_NAME3),
            _ => null,
        };
        GameScene.selectedPlayData = selectedPlayData;
        SceneManager.LoadScene("GameScene");
    }
}
