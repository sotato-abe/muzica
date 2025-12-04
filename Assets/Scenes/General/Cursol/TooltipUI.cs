using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance { get; private set; }

    public RectTransform panel;
    [SerializeField] RectTransform cardSpace;
    [SerializeField] TargetItemWindow targetItemWindow;
    [SerializeField] TargetCommandWindow targetCommandWindow;

    int offset = 250;

    bool isRight = true;

    // void Awake() => Instance = this;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン切り替えても残す
        }
        else
        {
            Destroy(gameObject); // 重複防止
        }
        if (panel.position.x > Screen.width * 2 / 3) // 画面の右側にある場合
        {
            isRight = true;
        }
        else if (panel.position.x <= Screen.width / 3)
        {
            isRight = false;
        }
        switchCardPosition();

        targetItemWindow.gameObject.SetActive(false);
        targetCommandWindow.gameObject.SetActive(false);
    }

    void Update()
    {
        // panel.position = Input.mousePosition;
        StartCoroutine(MoveTooltip());

        // 自身の場所が画面の中心より右の時、カードスペースの位置を左側に変更
        if (panel.position.x > Screen.width * 2 / 3 && isRight == false) // 画面の右側にある場合
        {
            isRight = true;
            switchCardPosition();
        }
        else if (panel.position.x <= Screen.width / 3 && isRight == true)
        {
            isRight = false;
            switchCardPosition();
        }
    }

    public void TargetItem(Item item, bool isOwn = true)
    {
        targetItemWindow.gameObject.SetActive(true);
        targetItemWindow.TargetItem(item);
    }

    public void TargetCommand(Command command, bool isOwn = true)
    {
        targetCommandWindow.gameObject.SetActive(true);
        targetCommandWindow.TargetCommand(command, isOwn);
    }

    public void ClearTargetItem()
    {
        targetItemWindow.TargetItem(null);
        targetItemWindow.gameObject.SetActive(false);
    }
    public void ClearTargetCommand()
    {
        targetCommandWindow.TargetCommand(null);
        targetCommandWindow.gameObject.SetActive(false);
    }

    private IEnumerator MoveTooltip()
    {
        // カーソルを少し遅れて追従する処理
        Vector3 startPos = panel.position;
        Vector3 targetPos = Input.mousePosition;
        float duration = 0.1f; // 移動にかける時間
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = t * t * (3f - 2f * t); // スムーズステップ関数
            panel.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }
    }

    private void switchCardPosition()
    {
        StartCoroutine(SlideCardPosition(isRight));
    }

    private IEnumerator SlideCardPosition(bool targetRight)
    {
        yield return null; // 1フレーム待機
        Vector3 cardSpacePos = cardSpace.localPosition;
        cardSpacePos.x = isRight ? -Mathf.Abs(offset) : Mathf.Abs(offset);

        float duration = 0.2f; // スライドの所要時間
        float elapsed = 0f;
        Vector3 startPos = cardSpace.localPosition;
        Vector3 endPos = cardSpacePos;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cardSpace.localPosition = Vector3.Lerp(startPos, endPos, elapsed / duration);
            yield return null;
        }
    }

    public void Show(string content)
    {
        panel.gameObject.SetActive(true);
    }

    public void Hide()
    {
        panel.gameObject.SetActive(false);
    }
}
