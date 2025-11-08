using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class WorldMapController : MonoBehaviour
{
    // プレイヤーのポジションを管理
    // ポイントデータの取得：pointMapからプレイヤーポジションのタイルを取得し、PointDatabaseから対応するfielddDataを取得
    // フィールドデータの取得：groundMapからプレイヤーポジションのタイルを取得し、タイルのFieldTypeから対応するFieldTileSetを取得

    public static WorldMapController Instance { get; private set; }
    public TileBase playerIcon;
    public Vector2Int playerPosition;
    public Tilemap groundMap;
    public Tilemap fieldMap;
    public Tilemap pinMap;
    public FieldGenerator fieldGenerator; // 今あるフィールド生成スクリプト
    public Transform player;              // プレイヤー
    [SerializeField] private WorldMapCamera worldMapCamera;
    [SerializeField] private WorldBigMapCameraManager worldMapBigCamera;
    [SerializeField] WorldMapRender worldMapRender;
    [SerializeField] WorldMapPanel worldMapPanel;
    [SerializeField] AgeTimePanel ageTimePanel;
    [SerializeField] MessagePanel messagePanel;

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

    private void Start()
    {
        ChangePlayerCoordinate(DirectionType.Other);
        Vector2Int startPos = Vector2Int.up;
    }

    public void WarpPlayerCoordinate(Vector2Int targetPosition)
    {
        // すでにイベント実行中のときはワールド移動をスキップする。
        if (PlayerController.Instance.CurrentEventType != EventType.Default)
            return;

        playerPosition = targetPosition;
        RenderFieldMap();
        ChangeFieldPlayerPosition(DirectionType.Other); // フィールドのプレイヤー位置を更新
        SetWorldMapPlayerPosition();
    }

    public void ChangePlayerCoordinate(DirectionType direction)
    {
        // すでにイベント実行中のときはワールド移動をスキップする。
        if (PlayerController.Instance.CurrentEventType != EventType.Default)
            return;

        playerPosition = playerPosition + direction.GetDirectionVector2Int();
        RenderFieldMap();
        ChangeFieldPlayerPosition(direction.GetOppositeDirection()); // フィールドのプレイヤー位置を更新
        SetWorldMapPlayerPosition();
        ageTimePanel.PassageOfMonth(2); // 2ヶ月進める
    }

    private void RenderFieldMap()
    {
        FieldTileSet fieldTileSet = GetTileSet(playerPosition);
        FieldBase fieldBase = GetFieldBase(playerPosition);

        // WorldMapで周りのfieldを確認して、fieldがあればfieldBaseのisTopOpenとかを設定する
        fieldBase.fieldType = fieldTileSet.FieldType;
        fieldBase.isTopOpen = worldMapRender.HasFieldMap(playerPosition + Vector2Int.up);
        fieldBase.isBottomOpen = worldMapRender.HasFieldMap(playerPosition + Vector2Int.down);
        fieldBase.isRightOpen = worldMapRender.HasFieldMap(playerPosition + Vector2Int.right);
        fieldBase.isLeftOpen = worldMapRender.HasFieldMap(playerPosition + Vector2Int.left);
        if (fieldBase.FieldName != "")
        {
            worldMapPanel.SetFieldName(fieldBase.FieldName);
            // messagePanel.AddMessage(MessageIconType.Field, fieldBase.FieldName + "に到着した");
            InformationPanel.Instance.SetFieldInformation(fieldBase);
        }
        else
        {
            worldMapPanel.SetFieldName("");
        }
        FieldController.Instance.SetField(fieldBase);
        fieldGenerator.SetField(fieldBase, fieldTileSet);
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

    private FieldBase GetFieldBase(Vector2Int targetPosition)
    {
        FieldBase fieldBase = FieldDatabase.Instance.GetFieldBaseByCoordinate(targetPosition);

        return fieldBase;
    }

    private void ChangeFieldPlayerPosition(DirectionType direction)
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
                SetWorldCameraPosition();
                // Debug.Log($"Player icon set at position {playerPosition.x}/{playerPosition.y} in pinMap.");
            }
            else
            {
                Debug.LogWarning("pinMap is not assigned in WorldMapController.");
            }
        }
    }

    // playerIconを中心視するようにworldMapCameraを移動
    private void SetWorldCameraPosition()
    {
        if (worldMapCamera == null)
        {
            worldMapCamera = Camera.main.GetComponent<WorldMapCamera>(); // 必要なら自動取得
            if (worldMapCamera == null)
            {
                Debug.LogWarning("WorldMapCamera is not assigned or not found on main camera.");
                return;
            }
        }

        Vector3Int tilePos = new Vector3Int(playerPosition.x, playerPosition.y, 0);
        Vector3 worldPos = pinMap.GetCellCenterWorld(tilePos);

        // カメラの位置をプレイヤーのアイコンに移動（Zは固定）
        Vector3 camPos = new Vector3(worldPos.x, worldPos.y, worldMapCamera.transform.position.z);
        worldMapCamera.transform.position = camPos;

        Vector3Int worldPosInt = new Vector3Int((int)worldPos.x, (int)worldPos.y, -10);
        worldMapBigCamera.TargetPlayer(worldPosInt);
    }
}