using UnityEngine;

public class FieldTransitionManager : MonoBehaviour
{
    public static FieldTransitionManager Instance { get; private set; }

    public FieldGenerator fieldGenerator; // 今あるフィールド生成スクリプト
    public Transform player;              // プレイヤー
    public Vector3 fieldSpawnPoint = Vector3.zero;

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

        // プレイヤーを初期位置に移動
        player.position = fieldSpawnPoint;
    }
}
