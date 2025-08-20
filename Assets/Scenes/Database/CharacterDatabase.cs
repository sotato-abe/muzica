using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDatabase : MonoBehaviour
{
    public static CharacterDatabase Instance { get; private set; }
    [SerializeField] List<CharacterBase> characterDataList;

    public CharacterBase LoadCharacterData(int characterId)
    {
        if (characterId < 0 || characterId >= characterDataList.Count)
        {
            Debug.LogError("Invalid character ID: " + characterId);
            return null;
        }

        CharacterBase character = characterDataList[characterId];
        return character;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetCharacterId(CharacterBase character)
    {
        return characterDataList.IndexOf(character);
    }
}