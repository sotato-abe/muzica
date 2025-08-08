using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target; // 追跡対象（プレイヤー）
    public float smoothSpeed = 5f; // 追従のなめらかさ
    public Vector3 offset; // 追跡位置のズレ

    private Vector3 defaultPosition = new Vector3(0, 0, 0); // 通常時のカメラ位置
    private Vector3 battlePosition = new Vector3(0, -1, 0); // バトル時のカメラ位置
    private Vector3 tradePosition = new Vector3(0, 4, 0); // 取引時のカメラ位置
    private Vector3 reservePosition = new Vector3(0, 4, 0); // 準備時のカメラ位置

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        desiredPosition.z = transform.position.z;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    public void SetEventType(EventType type)
    {
        switch (type)
        {
            case EventType.Battle:
                offset = battlePosition;
                break;
            case EventType.Trade:
                offset = tradePosition;
                break;
            case EventType.Reserve:
                offset = reservePosition;
                break;
            default:
                offset = defaultPosition;
                break;
        }
    }
}