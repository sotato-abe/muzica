using UnityEngine;

public class FieldTransitionManager : MonoBehaviour
{
    public static FieldTransitionManager Instance { get; private set; }

    public FieldGenerator fieldGenerator; // 今あるフィールド生成スクリプト
    public Transform player;              // プレイヤー
    public Vector3 fieldSpawnPoint = Vector3.zero;

    private void Start()
    {
        // 初期フィールドをロード
        // LoadField();
        fieldGenerator.SetField();
        SetPlayerPosition();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void LoadField()
    {
        Debug.Log($"LoadField");

        // 古いフィールド削除（必要なら）
        fieldGenerator.ClearField();

        // 新しいフィールドを生成
        fieldGenerator.GenerateField();
        // ゲートの位置を取得してその位置をプレイヤーの初期位置に設定
        Vector2Int spawnPos = fieldGenerator.GetEntorancePosition();
        Vector3Int cell = new Vector3Int(spawnPos.x, spawnPos.y, 0);
        player.position = fieldGenerator.tilemap.GetCellCenterWorld(cell);
    }

    private void SetPlayerPosition()
    {
        if (player != null)
        {
            Vector2Int spawnPos = fieldGenerator.GetEntorancePosition();
            Vector3Int cell = new Vector3Int(spawnPos.x, spawnPos.y, 0);
            player.position = fieldGenerator.tilemap.GetCellCenterWorld(cell);
        }
    }
}
