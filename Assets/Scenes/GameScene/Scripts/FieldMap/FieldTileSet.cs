using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewFieldTileSet", menuName = "Field/FieldTileSet")]
public class FieldTileSet : ScriptableObject
{
    [SerializeField] FieldType fieldType = FieldType.Default;
    [SerializeField] TileBase groundTile;
    [SerializeField] TileBase areaTile;
    [SerializeField] GameObject[] objectPrefabs;

    public FieldType FieldType { get => fieldType; }
    public TileBase GroundTile { get => groundTile; }
    public TileBase AreaTile { get => areaTile; }
    public GameObject[] ObjectPrefabs { get => objectPrefabs; }
}
