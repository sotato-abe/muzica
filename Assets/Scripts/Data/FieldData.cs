using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewFieldData", menuName = "Field/FieldData")]
public class FieldData : ScriptableObject
{
    [SerializeField] int fieldHeight = 50;
    [SerializeField] int fieldWidth = 50;
    [SerializeField] TileBase groundTile;
    [SerializeField] float fillPercent = 0.4f; // マップの建蔽率
    [SerializeField] float objectPercent = 0.4f; // マップの建蔽率

    public int FieldHeight { get => fieldHeight; }
    public int FieldWidth { get => fieldWidth; }
    public TileBase GroundTile { get => groundTile; }
    public float FillPercent { get => fillPercent; }
    public float ObjectPercent { get => objectPercent; }
}
