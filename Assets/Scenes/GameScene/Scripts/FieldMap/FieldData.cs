using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewFieldData", menuName = "Field/FieldData")]
public class FieldData : ScriptableObject
{
    [SerializeField] Vector2Int position = new Vector2Int(0, 0); // フィールドの位置
    [SerializeField] Sprite icon; // ワールドマップで表示されるアイコン
    [SerializeField] string fieldName = "";
    [SerializeField] int fieldHeight = 40;
    [SerializeField] int fieldWidth = 30;
    [SerializeField] FieldTileSet fieldTileSet;
    [SerializeField] float groundFillPercent = 0.4f; // マップの建蔽率
    [SerializeField] float areaFillPercent = 0.2f; // マップの建蔽率
    [SerializeField] int objectCount = 5;
    [SerializeField] List<PointBase> points = new List<PointBase>();
    [SerializeField] List<ItemBase> treasureBoxItems = new List<ItemBase>();
    [SerializeField] List<CharacterGroup> enemyGroups = new List<CharacterGroup>();
    public bool isTopOpen = false;
    public bool isBottomOpen = false;
    public bool isRightOpen = false;
    public bool isLeftOpen = false;

    public Vector2Int Position { get => position; }
    public Sprite Icon { get => icon; }
    public string FieldName { get => fieldName; }
    public int FieldHeight { get => fieldHeight; }
    public int FieldWidth { get => fieldWidth; }
    public FieldTileSet FieldTileSet { get => fieldTileSet; }
    public float GroundFillPercent { get => groundFillPercent; }
    public float AreaFillPercent { get => areaFillPercent; }
    public int ObjectCount { get => objectCount; }
    public List<PointBase> Points { get => points; }
    public List<ItemBase> TreasureBoxItems { get => treasureBoxItems; }
    public List<CharacterGroup> EnemyGroups { get => enemyGroups; }
}
