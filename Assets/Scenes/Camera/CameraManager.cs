using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target; // 追跡対象（プレイヤー）
    public float smoothSpeed = 5f; // 追従のなめらかさ
    public Vector3 offset; // 追跡位置のズレ

    private Vector3 defaultPosition = new Vector3(-1.0f, 0, 0); // 通常時のカメラ位置
    private Vector3 battlePosition = new Vector3(-2, -0.5f, 0); // バトル時のカメラ位置
    private Vector3 tradePosition = new Vector3(0, 2.5f, 0); // 取引時のカメラ位置
    private Vector3 reservePosition = new Vector3(0, 2.5f, 0); // 準備時のカメラ位置

    private float defaultSize = 5f; // 通常時のカメラサイズ
    private float scaleUpSize = 3.5f; // バトル時のカメラ

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
                StartCoroutine(ChangeScale(scaleUpSize));
                break;
            case EventType.Trade:
                offset = tradePosition;
                StartCoroutine(ChangeScale(scaleUpSize));
                break;
            case EventType.Reserve:
                offset = reservePosition;
                StartCoroutine(ChangeScale(scaleUpSize));
                break;
            default:
                offset = defaultPosition;
                StartCoroutine(ChangeScale(defaultSize));
                break;
        }
    }

    private IEnumerator ChangeScale(float targetSize)
    {
        Camera cam = GetComponent<Camera>();
        float startSize = cam.orthographicSize;
        float elapsed = 0f;
        float duration = 0.4f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // イージング (EaseInOutQuad)
            t = t * t * (3f - 2f * t);

            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, t);
            yield return null;
        }

        cam.orthographicSize = targetSize;
    }
}