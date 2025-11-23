using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDatabase : MonoBehaviour
{
    public static CharacterDatabase Instance { get; private set; }

    [SerializeField] private List<PlayCharacterBase> playerDataList;
    [SerializeField] private List<CharacterBase> ownerDataList;
    [SerializeField] private List<CharacterBase> enemyDataList;

    [SerializeField] List<EnemyGroup> defaultEnemyGroups = new List<EnemyGroup>();
    [SerializeField] List<EnemyGroup> desertEnemyGroups = new List<EnemyGroup>();
    [SerializeField] List<EnemyGroup> wildernessEnemyGroups = new List<EnemyGroup>();
    [SerializeField] List<EnemyGroup> grasslandsEnemyGroups = new List<EnemyGroup>();
    [SerializeField] List<EnemyGroup> wetlandsEnemyGroups = new List<EnemyGroup>();
    [SerializeField] List<EnemyGroup> snowEnemyGroups = new List<EnemyGroup>();
    [SerializeField] List<EnemyGroup> rockEnemyGroups = new List<EnemyGroup>();
    [SerializeField] List<EnemyGroup> magmaEnemyGroups = new List<EnemyGroup>();
    [SerializeField] List<EnemyGroup> pollutionEnemyGroups = new List<EnemyGroup>();
    [SerializeField] List<EnemyGroup> seaEnemyGroups = new List<EnemyGroup>();
    [SerializeField] List<EnemyGroup> oceanEnemyGroups = new List<EnemyGroup>();
    [SerializeField] List<EnemyGroup> outfieldEnemyGroups = new List<EnemyGroup>();

    private Dictionary<FieldType, List<EnemyGroup>> fieldTypeEnemyGroups;

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

        fieldTypeEnemyGroups = new Dictionary<FieldType, List<EnemyGroup>>
        {
            { FieldType.Default, defaultEnemyGroups },
            { FieldType.Desert, desertEnemyGroups },
            { FieldType.Wilderness, wildernessEnemyGroups },
            { FieldType.Grasslands, grasslandsEnemyGroups },
            { FieldType.Wetlands, wetlandsEnemyGroups },
            { FieldType.Snow, snowEnemyGroups },
            { FieldType.Rock, rockEnemyGroups },
            { FieldType.Magma, magmaEnemyGroups },
            { FieldType.Pollution, pollutionEnemyGroups },
            { FieldType.Sea, seaEnemyGroups },
            { FieldType.Ocean, oceanEnemyGroups }
        };
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
        allCharacterDataList.AddRange(playerDataList.ConvertAll(pc => pc.Base));
        allCharacterDataList.AddRange(ownerDataList);
        allCharacterDataList.AddRange(enemyDataList);
        return allCharacterDataList;
    }

    public PlayCharacterBase GetCharacterByIndex(CharacterIndex characterIndex)
    {
        return playerDataList.Find(pc => pc.CharacterIndex == characterIndex);
    }

    // 複数の敵をグループから取得するように変更する
    public List<Character> GetFieldEnemies(FieldType fieldType)
    {
        List<Character> enemies = new List<Character>();
        // fieldTypeが一致するEnemyGroupの中からランダムに選択して敵を取得する処理を実装する
        if (fieldTypeEnemyGroups.TryGetValue(fieldType, out List<EnemyGroup> enemyGroups))
        {
            if (enemyGroups.Count > 0)
            {
                // ランダムにEnemyGroupを選択
                EnemyGroup selectedGroup = enemyGroups[Random.Range(0, enemyGroups.Count)];
                List<EnemyCharacter> selectedEnemies = selectedGroup.GetRandomCharacterList();
                foreach (var enemyChar in selectedEnemies)
                {
                    enemies.Add(enemyChar);
                }
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning("No enemy groups found for field type: " + fieldType);
        }

        return enemies;
    }
}
