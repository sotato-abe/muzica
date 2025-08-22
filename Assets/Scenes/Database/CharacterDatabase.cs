using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDatabase : MonoBehaviour
{
    public static CharacterDatabase Instance { get; private set; }

    [SerializeField] private List<CharacterBase> characterDataList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン切り替えても残す
        }
        else
        {
            Destroy(gameObject); // 重複防止
        }
    }

    public CharacterBase LoadCharacterData(int characterId)
    {
        if (characterId < 0 || characterId >= characterDataList.Count)
        {
            Debug.LogError("Invalid character ID: " + characterId);
            return null;
        }

        return characterDataList[characterId];
    }

    public Character GetCharacterFromId(int characterId)
    {
        if (characterId < 0 || characterId >= characterDataList.Count)
        {
            Debug.LogError("Invalid character ID: " + characterId);
            return null;
        }

        CharacterBase baseData = characterDataList[characterId];
        Character loadCharacter = new Character(baseData);
        Debug.Log($"GetCharacterFromId: {loadCharacter.Base.name}, ID: {characterId}");
        return loadCharacter;
    }

    public PlayerCharacter GetPlayerCharacterFromId(int characterId)
    {
        if (characterId < 0 || characterId >= characterDataList.Count)
        {
            Debug.LogError("Invalid character ID: " + characterId);
            return null;
        }

        CharacterBase baseData = characterDataList[characterId];
        PlayerCharacter loadCharacter = new PlayerCharacter(baseData);
        return loadCharacter;
    }

    public int GetCharacterId(CharacterBase character)
    {
        return characterDataList.IndexOf(character);
    }
}
