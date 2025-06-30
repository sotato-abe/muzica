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
        ChangePlayerCoordinate(playerPosition);
        Vector2Int startPos = Vector2Int.up;
    }

    public void ChangePlayerCoordinate(Vector2Int direction)
    {
        playerPosition = playerPosition + direction;
        FieldTileSet tileSet = GetTileSet(playerPosition);
        FieldData fieldData = GetFieldData(playerPosition);

        fieldGenerator.SetField(fieldData, tileSet, playerPosition.x + "," + playerPosition.y);
        SetFieldPlayerPosition(direction);
        SetWorldMapPlayerPosition();
        Debug.Log($"Player position changed to {playerPosition}");
    }

    private FieldTileSet GetTileSet(Vector2Int targetPosition)
    {
        TileBase tile = groundMap.GetTile(new Vector3Int(targetPosition.x, targetPosition.y, 0));
        if (tile == null)
        {
            Debug.LogWarning($"No tile found at position {targetPosition} in groundMap!");
            return null;
        }

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

    private void SetFieldPlayerPosition(Vector2Int direction)
    {
        if (player != null)
        {
            Vector3Int playerPos = fieldGenerator.GetEntrancePosition(direction);
            player.position = fieldGenerator.Tilemap.GetCellCenterWorld(playerPos);
        }
    }

    private void SetWorldMapPlayerPosition()
    {
        // pinMap上にプレイヤーの位置を表示
        if (player != null)
        {
            Vector3Int cell = new Vector3Int(playerPosition.x, playerPosition.y, 0);
            pinMap.ClearAllTiles(); // 既存のタイルをクリア
            if (pinMap != null)
            {
                pinMap.SetTile(cell, playerIcon);
                Debug.Log($"Player icon set at position {playerPosition.x}/{playerPosition.y} in pinMap.");
            }
            else
            {
                Debug.LogWarning("pinMap is not assigned in WorldMapController.");
            }
        }
    }
}