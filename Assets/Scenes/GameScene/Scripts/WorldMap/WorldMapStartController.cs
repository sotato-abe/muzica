using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class WorldMapStartController : MonoBehaviour
{
    // プレイヤーのポジションを管理
    // ポイントデータの取得：pointMapからプレイヤーポジションのタイルを取得し、PointDatabaseから対応するfielddDataを取得
    // フィールドデータの取得：groundMapからプレイヤーポジションのタイルを取得し、タイルのFieldTypeから対応するFieldTileSetを取得

    public static WorldMapStartController Instance { get; private set; }
    public Vector2Int targetPosition;
    public Tilemap groundMap;
    public Tilemap fieldMap;
    public Tilemap pinMap;
    [SerializeField] private WorldBigMapCameraManager worldMapBigCamera;
    [SerializeField] WorldMapRender worldMapRender;
    [SerializeField] MapLibraryWindows mapLibraryWindows;

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
        mapLibraryWindows.OnChangeTarget += SelectTargetPosition;
        Vector2Int startPos = Vector2Int.up;
        SelectTargetPosition(0);
    }

    private void RenderFieldMap()
    {
        FieldTileSet fieldTileSet = GetTileSet(targetPosition);
        FieldBase fieldBase = GetFieldBase(targetPosition);

        // WorldMapで周りのfieldを確認して、fieldがあればfieldBaseのisTopOpenとかを設定する
        fieldBase.fieldType = fieldTileSet.FieldType;
        fieldBase.isTopOpen = worldMapRender.HasFieldMap(targetPosition + Vector2Int.up);
        fieldBase.isBottomOpen = worldMapRender.HasFieldMap(targetPosition + Vector2Int.down);
        fieldBase.isRightOpen = worldMapRender.HasFieldMap(targetPosition + Vector2Int.right);
        fieldBase.isLeftOpen = worldMapRender.HasFieldMap(targetPosition + Vector2Int.left);
        FieldController.Instance.SetField(fieldBase);
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

    public void SelectTargetPosition(int targetIndex)
    {
        Vector3Int tilePosition = worldMapRender.GetTargetSectorPosition(targetIndex);
        
        // タイル座標をワールド座標に変換（WorldMapControllerと同じ方式）
        Vector3 worldPos = fieldMap.GetCellCenterWorld(tilePosition);
        Vector3 cameraTarget = new Vector3(worldPos.x, worldPos.y, -10f);
        
        worldMapBigCamera.ChangeTarget(cameraTarget);
    }
}