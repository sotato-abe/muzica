using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class SaveWindow : Window
{
    [SerializeField] SaveDataButton saveDataButton1;
    [SerializeField] SaveDataButton saveDataButton2;
    [SerializeField] SaveDataButton saveDataButton3;
    [SerializeField] SaveManagement saveManagement;

    private List<SaveDataButton> saveDataButtons;

    public const string FILE_NAME = "playData";
    public const string FILE_EXTENSION = ".json";

    private void Start()
    {
        saveDataButton1.OnSaveGame += SaveGame;
        saveDataButton2.OnSaveGame += SaveGame;
        saveDataButton3.OnSaveGame += SaveGame;

        saveDataButtons = new List<SaveDataButton>
        {
            saveDataButton1,
            saveDataButton2,
            saveDataButton3
        };
        LoadData();
    }


    void SaveGame(int index)
    {
        saveManagement.SavePlayData(index);
        Debug.Log($"Game saved to slot {index}");
        LoadData();
    }

    private void LoadData()
    {
        for (int i = 0; i < saveDataButtons.Count; i++)
        {
            SimpleData simpleData = saveManagement.GetSaveData(i + 1);
            saveDataButtons[i].Setup(simpleData);
        }
    }
}
