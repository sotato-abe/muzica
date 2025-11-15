using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Panel : MonoBehaviour
{
    public UnityAction OnActionExecute;
    public UnityAction OnActionExit;

    // public ActionType type;
    public bool isActive = false;
    private bool isAnimating = false; // アニメーション中かどうかのフラグ
    private Coroutine openMotionCoroutine = null; // コルーチンの参照
    private float groundY; // 初期位置を保存

    protected virtual void Awake()
    {
        // 初期位置を保存
        groundY = transform.position.y;
    }

    public void SwitchActive()
    {
        isActive = !isActive; // フラグをトグル
        if (isActive)
        {
            PanelOpen();
        }
        else
        {
            PanelClose();
        }
    }

    public virtual void PanelOpen()
    {
        isActive = true;
        transform.gameObject.SetActive(isActive);
        if (!isAnimating) // 二重実行防止
        {
            openMotionCoroutine = StartCoroutine(OpenMotion());
        }
    }

    public virtual void PanelClose()
    {
        isActive = false;
        transform.gameObject.SetActive(isActive);
    }

    private IEnumerator OpenMotion()
    {
        isAnimating = true; // アニメーション開始

        float bounceHeight = 20f;
        float damping = 0.2f;
        float gravity = 5000f;

        while (bounceHeight >= 0.1f)
        {
            float verticalVelocity = Mathf.Sqrt(2 * gravity * bounceHeight);
            bool isFalling = false;

            // 上昇と下降のループ
            while (transform.position.y >= groundY || !isFalling)
            {
                verticalVelocity -= gravity * Time.deltaTime;
                transform.position += Vector3.up * verticalVelocity * Time.deltaTime;

                if (transform.position.y <= groundY)
                {
                    isFalling = true;
                    break;
                }

                yield return null;
            }

            bounceHeight *= damping; // バウンドを減衰させる
        }

        transform.position = new Vector3(transform.position.x, groundY, transform.position.z); // 最後に位置を調整
        isAnimating = false; // アニメーション終了
        openMotionCoroutine = null; // コルーチンの参照をリセット
    }

    private void OnDisable()
    {
        // GameObject が非アクティブになったらアニメーション状態をリセット
        isAnimating = false;

        // もしコルーチンが動いていたら停止する
        if (openMotionCoroutine != null)
        {
            transform.position = new Vector3(transform.position.x, groundY, transform.position.z);
            StopCoroutine(openMotionCoroutine);
            openMotionCoroutine = null;
        }
    }
}
