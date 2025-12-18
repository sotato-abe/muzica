using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewFieldData", menuName = "Field/FieldData")]
public class FieldBase : ScriptableObject
{
    [Header("Field settings")]
    [SerializeField] Vector2Int position = new Vector2Int(0, 0); // フィールドの位置
    [SerializeField] int fieldHeight = 40;
    [SerializeField] int fieldWidth = 30;
    [SerializeField] FieldTileSet fieldTileSet;
    [SerializeField] float groundFillPercent = 0.4f; // マップの建蔽率
    [SerializeField] float areaFillPercent = 0.2f; // マップの建蔽率
    [SerializeField] string seed;

    [Header("Field information")]
    [SerializeField] Sprite icon; // ワールドマップで表示されるアイコン
    [SerializeField] string fieldName = "";
    [SerializeField] Sprite fieldSprite; // フィールドのスプライト
    [SerializeField] AudioClip secterBgm;
    [SerializeField] AudioClip ambient;
    [SerializeField, TextArea] string description;

    [Header("Objects")]
    [SerializeField] List<PointBase> points = new List<PointBase>();
    [SerializeField] List<ItemBase> treasureBoxItems = new List<ItemBase>();

    [Header("Enemies")]
    [SerializeField] List<EnemyGroup> enemyGroups = new List<EnemyGroup>();

    // Setting
    public Vector2Int Position { get => position; }
    public Vector2Int currentPosition;
    public int FieldHeight { get => fieldHeight; }
    public int FieldWidth { get => fieldWidth; }
    public FieldTileSet FieldTileSet { get => fieldTileSet; }
    public float GroundFillPercent { get => groundFillPercent; }
    public float AreaFillPercent { get => areaFillPercent; }
    public string Seed { get => seed; }
    public FieldType fieldType = FieldType.Default; // フィールドの種類
    public bool isTopOpen = false;
    public bool isBottomOpen = false;
    public bool isRightOpen = false;
    public bool isLeftOpen = false;

    // Field information
    public Sprite Icon { get => icon; }
    public string FieldName { get => fieldName; }
    public Sprite FieldSprite { get => fieldSprite; }
    public AudioClip SecterBgm { get => secterBgm; }
    public AudioClip Ambient { get => ambient; }
    public string Description { get => description; }

    // Objects
    public List<PointBase> Points { get => points; }
    public List<ItemBase> TreasureBoxItems { get => treasureBoxItems; }
    public List<EnemyGroup> EnemyGroups { get => enemyGroups; }
}
