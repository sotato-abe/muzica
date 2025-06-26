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
    [SerializeField] GameObject[] objectPrefabs;

    public TileBase GroundTile { get => groundTile; }
    public TileBase GrassTile { get => grassTile; }
    public GameObject[] ObjectPrefabs { get => objectPrefabs; }
}
