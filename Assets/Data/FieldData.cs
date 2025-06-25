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
    [SerializeField] TileBase gateTile;
    [SerializeField] float fillPercent = 0.4f; // マップの建蔽率
    [SerializeField] float objectPercent = 0.4f; // マップの建蔽率

    [SerializeField] bool isTopOpen = false;
    [SerializeField] bool isBottomOpen = false;
    [SerializeField] bool isRightOpen = false;
    [SerializeField] bool isLeftOpen = false;

    public int FieldHeight { get => fieldHeight; }
    public int FieldWidth { get => fieldWidth; }
    public TileBase GroundTile { get => groundTile; }
    public TileBase GateTile { get => gateTile; }
    public float FillPercent { get => fillPercent; }
    public float ObjectPercent { get => objectPercent; }

    public bool IsTopOpen { get => isTopOpen; }
    public bool IsBottomOpen { get => isBottomOpen; }
    public bool IsRightOpen { get => isRightOpen; }
    public bool IsLeftOpen { get => isLeftOpen; }
}
