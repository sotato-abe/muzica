using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDatabase : MonoBehaviour
{
    public static CharacterDatabase Instance { get; private set; }

    [SerializeField] private List<PlayCharacterBase> playerDataList;
    [SerializeField] private List<CharacterBase> ownerDataList;
    [SerializeField] private List<CharacterBase> enemyDataList;

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

    public PlayerCharacter GetPlayerCharacterFromId(int characterId)
    {
        if (characterId < 0 || characterId >= playerDataList.Count)
        {
            Debug.LogError("Invalid character ID: " + characterId);
            return null;
        }

        CharacterBase baseData = playerDataList[characterId].Base;
        PlayerCharacter loadCharacter = new PlayerCharacter(baseData);
        return loadCharacter;
    }

    public int GetCharacterId(CharacterBase character)
    {
        return playerDataList.FindIndex(pc => pc.Base == character);
    }

    public List<CharacterBase> GetAllCharacterBases()
    {
        List<CharacterBase> allCharacterDataList = new List<CharacterBase>();
        allCharacterDataList.AddRange( playerDataList.ConvertAll(pc => pc.Base));
        allCharacterDataList.AddRange(ownerDataList);
        allCharacterDataList.AddRange(enemyDataList);
        return allCharacterDataList;
    }

    public PlayCharacterBase GetCharacterByIndex(CharacterIndex characterIndex)
    {
        return playerDataList.Find(pc => pc.CharacterIndex == characterIndex);
    }
}
