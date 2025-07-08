using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Motion : MonoBehaviour
{
    private Vector3 originalPosition;

    private void Start()
    {
        // 初期位置を保存
        originalPosition = transform.localPosition;
        // モーション開始
        StartCoroutine(MoveMotion());
    }

    private void OnEnable()
    {
        // モーション開始
        StartCoroutine(MoveMotion());
    }

    private IEnumerator MoveMotion()
    {
        float moveRange = 20f;  // 移動範囲
        float moveSpeed = 3f;  // 移動スピード
        float updateInterval = 5.0f; // ターゲット更新の時間間隔
        float elapsedTime = 0f;

        Vector3 targetPosition = originalPosition + new Vector3(
            Random.Range(-moveRange, moveRange),
            Random.Range(-moveRange, moveRange),
            0
        );
        Quaternion targetRotation = transform.rotation; // 目標の回転

        while (true)
        {
            // 一定時間経過したらターゲット更新
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= updateInterval || Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                targetPosition = originalPosition + new Vector3(
                    Random.Range(-moveRange, moveRange),
                    Random.Range(-moveRange, moveRange),
                    0
                );
                targetRotation = Quaternion.Euler(0, 0, Random.Range(0, 3));
                elapsedTime = 0f;
            }

            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);

            yield return null; // 毎フレーム実行
        }
    }
}
