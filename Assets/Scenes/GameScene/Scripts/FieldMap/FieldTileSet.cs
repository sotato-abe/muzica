using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewFieldTileSet", menuName = "Field/FieldTileSet")]
public class FieldTileSet : ScriptableObject
{
    [SerializeField] FieldType fieldType = FieldType.Default;
    [SerializeField] TileBase groundTile;
    [SerializeField] TileBase grassTile;
    [SerializeField] TileBase objectTile;
    [SerializeField] TileBase objectTile2;
    public TileBase GroundTile { get => groundTile; }
    public TileBase GrassTile { get => grassTile; }
    public TileBase ObjectTile { get => objectTile; }
    public TileBase ObjectTile2 { get => objectTile2; }
}
