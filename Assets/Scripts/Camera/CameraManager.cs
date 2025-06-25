using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target; // 追跡対象（プレイヤー）
    public float smoothSpeed = 5f; // 追従のなめらかさ
    public Vector3 offset; // 追跡位置のズレ

    void LateUpdate()
    {
        if (target == null) return;


        Vector3 desiredPosition = target.position + offset;

        // Z軸だけ固定してカメラが後ろに回らないようにする
        desiredPosition.z = transform.position.z;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}