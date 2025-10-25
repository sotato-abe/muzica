using UnityEngine;

public class TreasureBoxPrefab : FieldTriggerPrefab
{
    public override void EnterAction()
    {
        FieldController.Instance.OpenTreasureBox();
        Destroy(gameObject);
    }
}
