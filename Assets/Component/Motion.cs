using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Motion : MonoBehaviour
{
    private Vector3 originalPosition;
    private Coroutine currentMotionCoroutine; // 現在実行中のモーションコルーチン
    private Coroutine defaultMotionCoroutine; // デフォルトモーション（Move3DMotion）のコルーチン
    
    // Move3DMotionの状態保存用
    private float savedTime = 0f;
    private float savedXOffset = 0f;
    private float savedYOffset = 0f;
    private float savedZOffset = 0f;
    private bool isFirstTime = true;

    private void Start()
    {
        // 初期位置を保存
        originalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        // デフォルトモーション開始
        StartDefaultMotion();
    }
    
    // デフォルトモーションを開始するメソッド
    public void StartDefaultMotion()
    {
        if (defaultMotionCoroutine != null)
        {
            StopCoroutine(defaultMotionCoroutine);
        }
        defaultMotionCoroutine = StartCoroutine(Move3DMotion());
    }
    
    // デフォルトモーションを停止するメソッド
    public void StopDefaultMotion()
    {
        if (defaultMotionCoroutine != null)
        {
            StopCoroutine(defaultMotionCoroutine);
            defaultMotionCoroutine = null;
        }
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

    private IEnumerator Move3DMotion()
    {
        // Sin波の周波数（低い値でゆっくりした動き）
        float xFrequency = 0.8f;
        float yFrequency = 0.4f;
        float zFrequency = 0.3f;

        // 回転の振幅（度数）
        float xAmplitude = 10f;  // X軸回転の最大角度
        float yAmplitude = 8f;  // Y軸回転の最大角度
        float zAmplitude = 2f;   // Z軸回転の最大角度

        // 初回のみランダムオフセットを設定、以降は保存した値を使用
        if (isFirstTime)
        {
            savedXOffset = Random.Range(0f, 2f * Mathf.PI);
            savedYOffset = Random.Range(0f, 2f * Mathf.PI);
            savedZOffset = Random.Range(0f, 2f * Mathf.PI);
            savedTime = 0f;
            isFirstTime = false;
        }

        float time = savedTime;

        while (true)
        {
            time += Time.deltaTime;
            savedTime = time; // 時間を保存

            // Sin波を使って連続的で滑らかな回転を計算
            float xRotation = Mathf.Sin(time * xFrequency + savedXOffset) * xAmplitude;
            float yRotation = Mathf.Sin(time * yFrequency + savedYOffset) * yAmplitude;
            float zRotation = Mathf.Sin(time * zFrequency + savedZOffset) * zAmplitude;

            // 位置は常に元の位置に保持
            transform.localPosition = originalPosition;

            // 連続的で滑らかな回転を適用
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, zRotation);

            yield return null; // 毎フレーム実行
        }
    }

    public IEnumerator Open3DMotion()
    {
        // 前のコルーチンが実行中なら停止
        if (currentMotionCoroutine != null)
        {
            StopCoroutine(currentMotionCoroutine);
        }
        
        // デフォルトモーションを一時停止
        StopDefaultMotion();
        float jumpDuration = 0.3f;   // はねる時間
        float bounceDuration = 0.6f; // バウンスして戻る時間

        // 開始時点の角度を保存
        Quaternion startRotation = transform.localRotation;
        
        // 基準回転を0度（identity）に固定
        // Quaternion startRotation = Quaternion.identity;
        
        // 固定の回転ターゲット
        Vector3 targetRotation = new Vector3(
            10f,  // X軸回転
            20f,  // Y軸回転
            5f    // Z軸回転
        );
        
        Quaternion endRotation = Quaternion.Euler(targetRotation);
        
        // 連続呼び出し対応：開始時に即座に原点状態にリセット
        Vector3 originalScale = Vector3.one; // 基準スケールを固定値に
        
        // フェーズ1: 元の位置から最大回転位置まで素早くはねる（スケールも変化）
        float elapsedTime = 0f;
        while (elapsedTime < jumpDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / jumpDuration;
            
            // はねる瞬間のスケール変化（1.0 → 1.2 → 1.0）
            float scaleProgress = Mathf.Sin(progress * Mathf.PI);
            float scaleMultiplier = 1f + scaleProgress * 0.06f; // 最大6%拡大
            
            // 位置は常に元の位置に保持
            transform.localPosition = originalPosition;
            transform.localScale = originalScale * scaleMultiplier;
            
            // 元の回転から最大回転まで素早く移動
            transform.localRotation = Quaternion.Lerp(startRotation, endRotation, progress);
            
            yield return null;
        }
        
        // フェーズ2: 最大回転から元の回転にブルンとバウンドしながら戻る
        elapsedTime = 0f;
        while (elapsedTime < bounceDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / bounceDuration;
            
            // ブルンとしたバウンド
            float bounceProgress = BouncyEase(progress);
            
            // 位置は常に元の位置に保持
            transform.localPosition = originalPosition;
            transform.localScale = originalScale; // スケールを元に戻す
            
            // バウンドしながら元の回転に戻る
            transform.localRotation = Quaternion.Lerp(endRotation, startRotation, bounceProgress);
            
            yield return null;
        }
        
        // Move3DMotionの現在の角度を計算
        float xFrequency = 0.8f;
        float yFrequency = 0.4f;
        float zFrequency = 0.3f;
        float xAmplitude = 10f;
        float yAmplitude = 8f;
        float zAmplitude = 2f;
        
        float targetXRotation = Mathf.Sin(savedTime * xFrequency + savedXOffset) * xAmplitude;
        float targetYRotation = Mathf.Sin(savedTime * yFrequency + savedYOffset) * yAmplitude;
        float targetZRotation = Mathf.Sin(savedTime * zFrequency + savedZOffset) * zAmplitude;
        Quaternion targetMotionRotation = Quaternion.Euler(targetXRotation, targetYRotation, targetZRotation);
        
        // 現在の回転からMove3DMotionの回転に滑らかに移行する
        float transitionDuration = 0.3f;
        float transitionTime = 0f;
        Quaternion currentRotation = transform.localRotation;
        
        while (transitionTime < transitionDuration)
        {
            transitionTime += Time.deltaTime;
            float progress = transitionTime / transitionDuration;
            
            transform.localPosition = originalPosition;
            transform.localRotation = Quaternion.Lerp(currentRotation, targetMotionRotation, progress);
            
            yield return null;
        }
        
        // 最終的に元の状態に確実に戻す
        transform.localPosition = originalPosition;
        
        // コルーチン参照をクリア
        currentMotionCoroutine = null;
        
        UnityEngine.Debug.Log("Open3DMotion completed.");
        // デフォルトモーションを再開
        StartDefaultMotion();
    }
    
    // ブルンとしたバウンド関数
    private float BouncyEase(float t)
    {
        // ブルンとした動きのパラメータ
        float bounceCount = 2.5f;   // バウンド回数を少なく
        float bounceHeight = 0.4f;  // バウンドの高さ
        float damping = 3f;         // 適度な減衰
        
        // より自然なバウンド曲線
        float bounce = Mathf.Exp(-damping * t) * Mathf.Sin(bounceCount * Mathf.PI * t) * bounceHeight;
        
        // EaseOutQuart + バウンス効果でブルンとした動き
        float easeOut = 1f - Mathf.Pow(1f - t, 4f);
        return easeOut + bounce * (1f - easeOut);
    }
}
