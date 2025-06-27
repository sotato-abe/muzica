using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldMapController : MonoBehaviour
{
    // プレイヤーのポジションを管理
    // ポイントデータの取得：pointMapからプレイヤーポジションのタイルを取得し、PointDatabaseから対応するfielddDataを取得
    // フィールドデータの取得：groundMapからプレイヤーポジションのタイルを取得し、タイルのFieldTypeから対応するFieldTileSetを取得
    // FieldTransitionManagerの管理

    public static WorldMapController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public TileBase playerIcon;
    public Vector2Int playerPosition;
    public Tilemap groundMap;
    public Tilemap fieldMap;
    public Tilemap pinMap;
    public FieldGenerator fieldGenerator; // 今あるフィールド生成スクリプト
    public Transform player;              // プレイヤー

    private void Start()
    {
        // 初期フィールドをロード
        ChangePlayerCoordinate(playerPosition);
        SetPlayerPosition();
        SetMapPlayerPosition();
    }

    public void ChangePlayerCoordinate(Vector2Int newPosition)
    {
        playerPosition = playerPosition + newPosition;
        FieldTileSet tileSet = GetTileSet(playerPosition);
        FieldData fieldData = GetFieldData(playerPosition);

        fieldGenerator.SetField(fieldData, tileSet, playerPosition.x + "," + playerPosition.y);

        Vector2Int spawnPos = fieldGenerator.GetEntorancePosition();
        Vector3Int cell = new Vector3Int(spawnPos.x, spawnPos.y, 0);
        player.position = fieldGenerator.tilemap.GetCellCenterWorld(cell);
        SetMapPlayerPosition();
    }

    private FieldTileSet GetTileSet(Vector2Int targetPosition)
    {
        TileBase tile = groundMap.GetTile(new Vector3Int(targetPosition.x, targetPosition.y, 0));
        if (tile == null)
        {
            Debug.LogWarning($"No tile found at position {targetPosition} in groundMap!");
            return null;
        }
        Debug.Log($"Tile {tile}");

        FieldTileSet fieldTileSet = FieldTileSetDatabase.Instance.GetTileSetFromByTile(tile);

        if (fieldTileSet == null)
        {
            Debug.LogWarning($"No FieldTileSet found for tile {tile.name}");
        }

        return fieldTileSet;
    }

    private FieldData GetFieldData(Vector2Int targetPosition)
    {
        FieldData fieldData = FieldDatabase.Instance.GetFieldDataByCoordinate(targetPosition);

        return fieldData;
    }

    private void SetMapPlayerPosition()
    {
        // pinMap上にプレイヤーの位置を表示
        if (player != null)
        {
            Vector3Int cell = new Vector3Int(playerPosition.x, playerPosition.y, 0);
            pinMap.ClearAllTiles(); // 既存のタイルをクリア
            if (pinMap != null)
            {
                pinMap.SetTile(cell, playerIcon);
            }
            else
            {
                Debug.LogWarning("pinMap is not assigned in WorldMapController.");
            }
        }
    }

    private void SetPlayerPosition() // TODO　入口と反対側に移動するようにする
    {
        if (player != null)
        {
            Vector2Int spawnPos = fieldGenerator.GetEntorancePosition();
            Vector3Int cell = new Vector3Int(spawnPos.x, spawnPos.y, 0);
            player.position = fieldGenerator.tilemap.GetCellCenterWorld(cell);
        }
    }
}