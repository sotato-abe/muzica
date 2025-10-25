using UnityEngine;

public class GatePrefab : FieldTriggerPrefab
{
    public DirectionType directionType; // 移動方向を指定するための変数

    public override void EnterAction()
    {
        WorldMapController.Instance.ChangePlayerCoordinate(directionType);
    }
}
