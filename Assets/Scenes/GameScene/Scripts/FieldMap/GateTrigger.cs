using UnityEngine;

public class GateTrigger : MonoBehaviour
{
    public string destinationField; // 次のマップ名など（例："Field_B"）
    public FieldData nextField;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"ゲートに入った → 遷移先: {destinationField}");

            // フィールド遷移呼び出し（仮）
            FieldTransitionManager.Instance.LoadField();
        }
    }
}
