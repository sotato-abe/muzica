using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PlayData
{
    public DateTime time = DateTime.Now;
    public int yearsElapsed = 0;
    public Vector2Int position;
    public PlayerData playerData;
}

public class PlayerData
{
    public int characterId = 0;
    // status
    public int maxLife = 0;
    public int currentLife = 0;
    public int maxBattery = 0;
    public int currentBattery = 0;
    public int currentSoul = 0;
    public int power = 0;
    public int technique = 0;
    public int defense = 0;
    public int speed = 0;
    public int luck = 0;
    public int memory = 0;
    public int storage = 0;
    public int pocket = 0;

    // Currency
    public int coin = 0;
    public int disc = 0;
    public int key = 0;
    public int level = 0;
    public int skillPoint = 0;
    public int exp = 0;

    // Belongings
    public int rightEquipmentIndex = -1;
    public int leftEquipmentIndex = -1;
    public List<int> bagItemIndexList = new List<int>();
    public List<int> pocketItemIndexList = new List<int>();
    public List<int> storageCommandIndexList = new List<int>();
    public List<int> slotCommandIndexList = new List<int>();
}

public class SimpleData
{
    public string name;
    public int level;
    public DateTime time;
    public Vector2Int position;
    public string sceneName;
    public Sprite characterSprite;

    public SimpleData(PlayData playData)
    {
        PlayerCharacter loadPlayerCharacter = CharacterDatabase.Instance.GetPlayerCharacterFromId(playData.playerData.characterId);

        this.name = loadPlayerCharacter.Base.Name;
        this.level = loadPlayerCharacter.Level;
        this.time = playData.time;
        this.position = playData.position;
        this.sceneName = "test";
        this.characterSprite = loadPlayerCharacter.Base.SquareSprite;
    }
}