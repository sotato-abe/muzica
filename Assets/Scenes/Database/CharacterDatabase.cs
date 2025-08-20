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

    public int GetCharacterId(CharacterBase character)
    {
        return characterDataList.IndexOf(character);
    }
}
