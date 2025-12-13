using System.Collections;
using UnityEngine;

public class Motion : MonoBehaviour
{
    #region 設定構造体
    [System.Serializable]
    public struct Move3DSettings
    {
        public float xFrequency;
        public float yFrequency;
        public float zFrequency;
        public float xAmplitude;
        public float yAmplitude;
        public float zAmplitude;

        public static Move3DSettings Default => new Move3DSettings
        {
            xFrequency = 0.8f,
            yFrequency = 0.4f,
            zFrequency = 0.3f,
            xAmplitude = 10f,
            yAmplitude = 8f,
            zAmplitude = 2f
        };
    }

    [System.Serializable]
    public struct Open3DSettings
    {
        public float jumpDuration;
        public float transitionDuration;
        public Vector3 targetRotation;
        public float maxScaleMultiplier;
        public float jumpHeight;

        public static Open3DSettings Default => new Open3DSettings
        {
            jumpDuration = 0.3f,
            transitionDuration = 0.1f,
            targetRotation = new Vector3(10f, 20f, 5f),
            maxScaleMultiplier = 0.06f,
            jumpHeight = 10f
        };
    }
    #endregion

    #region フィールド
    [Header("Motion Settings")]
    [SerializeField] private Move3DSettings move3DSettings = Move3DSettings.Default;
    [SerializeField] private Open3DSettings open3DSettings = Open3DSettings.Default;

    // 内部状態
    private Vector3 originalPosition;
    private Coroutine currentMotionCoroutine;
    private Coroutine defaultMotionCoroutine;

    // Open3DMotion実行制御
    private bool isOpen3DMotionRunning = false;

    // Move3DMotionの状態保存用
    private MotionState motionState;

    private struct MotionState
    {
        public float time;
        public float xOffset;
        public float yOffset;
        public float zOffset;
        public bool isInitialized;
    }
    #endregion

    #region Unity Events
    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        StartDefaultMotion();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// デフォルトモーションを開始
    /// </summary>
    public void StartDefaultMotion()
    {
        StopCoroutineIfRunning(ref defaultMotionCoroutine);
        defaultMotionCoroutine = StartCoroutine(Move3DMotion());
    }

    /// <summary>
    /// デフォルトモーションを停止
    /// </summary>
    public void StopDefaultMotion()
    {
        StopCoroutineIfRunning(ref defaultMotionCoroutine);
    }

    /// <summary>
    /// Open3DMotionを安全に開始
    /// </summary>
    public void StartOpen3DMotion()
    {
        // 実行中フラグで確実に重複を防ぐ
        if (isOpen3DMotionRunning)
        {
            // 強制停止して新しいモーションを開始
            StopCoroutineIfRunning(ref currentMotionCoroutine);
            isOpen3DMotionRunning = false;
        }

        // 新しいOpen3DMotionを開始
        isOpen3DMotionRunning = true;
        currentMotionCoroutine = StartCoroutine(Open3DMotion());
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Initialize()
    {
        originalPosition = transform.localPosition;
        motionState = new MotionState
        {
            time = 0f,
            isInitialized = false
        };
    }

    /// <summary>
    /// コルーチンが実行中の場合停止
    /// </summary>
    private void StopCoroutineIfRunning(ref Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
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

    /// <summary>
    /// デフォルトの3D揺れモーション
    /// </summary>
    private IEnumerator Move3DMotion()
    {
        InitializeMotionStateIfNeeded();

        float time = motionState.time;

        while (true)
        {
            time += Time.deltaTime;
            motionState.time = time;

            var rotation = CalculateMove3DRotation(time);
            ApplyTransform(originalPosition, rotation, Vector3.one);

            yield return null;
        }
    }

    /// <summary>
    /// モーション状態を初期化（初回のみ）
    /// </summary>
    private void InitializeMotionStateIfNeeded()
    {
        if (!motionState.isInitialized)
        {
            motionState.xOffset = Random.Range(0f, 2f * Mathf.PI);
            motionState.yOffset = Random.Range(0f, 2f * Mathf.PI);
            motionState.zOffset = Random.Range(0f, 2f * Mathf.PI);
            motionState.time = 0f;
            motionState.isInitialized = true;
        }
    }

    /// <summary>
    /// Move3Dモーションの回転を計算
    /// </summary>
    private Quaternion CalculateMove3DRotation(float time)
    {
        var settings = move3DSettings;

        float xRotation = Mathf.Sin(time * settings.xFrequency + motionState.xOffset) * settings.xAmplitude;
        float yRotation = Mathf.Sin(time * settings.yFrequency + motionState.yOffset) * settings.yAmplitude;
        float zRotation = Mathf.Sin(time * settings.zFrequency + motionState.zOffset) * settings.zAmplitude;

        return Quaternion.Euler(xRotation, yRotation, zRotation);
    }

    /// <summary>
    /// Transform要素を一括適用
    /// </summary>
    private void ApplyTransform(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        transform.localPosition = position;
        transform.localRotation = rotation;
        transform.localScale = scale;
    }

    /// <summary>
    /// Open3D モーション（はね→バウンド→復帰）
    /// </summary>
    public IEnumerator Open3DMotion()
    {
        // 全てのモーションを停止
        StopDefaultMotion();

        // フェーズ1: ジャンプ（スケール変化付き）
        yield return StartCoroutine(ExecuteJumpPhase());

        // フェーズ2: 元の位置に直接戻る
        ApplyTransform(originalPosition, Quaternion.identity, Vector3.one);

        // フェーズ3: Move3DMotionへの滑らか移行
        yield return StartCoroutine(ExecuteTransitionPhase());

        // 完了処理
        CompleteOpen3DMotion();
    }

    /// <summary>
    /// ジャンプフェーズの実行
    /// </summary>
    private IEnumerator ExecuteJumpPhase()
    {
        float elapsedTime = 0f;
        var settings = open3DSettings;

        // ★ 常に基準状態から開始（前のモーションの影響を受けない）
        Vector3 startPosition = originalPosition;
        Vector3 startScale = Vector3.one;
        Quaternion startRotation = Quaternion.identity;
        Quaternion endRotation = Quaternion.Euler(settings.targetRotation);

        while (elapsedTime < settings.jumpDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / settings.jumpDuration);

            // Sin波ジャンプ
            float jump = Mathf.Sin(progress * Mathf.PI);

            // ★ 基準位置から上方向にジャンプ
            Vector3 position = startPosition + Vector3.up * jump * settings.jumpHeight;

            // ★ 基準スケールからスケール変化
            float scaleMultiplier = 1f + jump * settings.maxScaleMultiplier;
            Vector3 scale = startScale * scaleMultiplier;

            Quaternion rotation = Quaternion.Lerp(startRotation, endRotation, progress);

            ApplyTransform(position, rotation, scale);
            yield return null;
        }
    }

    /// <summary>
    /// Move3DMotionへの移行フェーズ
    /// </summary>
    private IEnumerator ExecuteTransitionPhase()
    {
        var settings = open3DSettings;
        Quaternion currentRotation = transform.localRotation;
        Quaternion targetMotionRotation = CalculateMove3DRotation(motionState.time);

        float transitionTime = 0f;

        while (transitionTime < settings.transitionDuration)
        {
            transitionTime += Time.deltaTime;
            float progress = transitionTime / settings.transitionDuration;

            var rotation = Quaternion.Lerp(currentRotation, targetMotionRotation, progress);
            ApplyTransform(originalPosition, rotation, Vector3.one);

            yield return null;
        }
    }

    /// <summary>
    /// Open3DMotion完了処理
    /// </summary>
    private void CompleteOpen3DMotion()
    {
        // 実行フラグをfalseに設定
        isOpen3DMotionRunning = false;

        ApplyTransform(originalPosition, Quaternion.identity, Vector3.one);
        currentMotionCoroutine = null;
        StartDefaultMotion();
    }
    #endregion
}
