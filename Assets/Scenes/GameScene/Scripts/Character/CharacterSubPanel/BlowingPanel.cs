using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlowingPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] RectTransform backRectTransform;
    [SerializeField] Image panelImage;
    [SerializeField] Sprite DefaultBackImage;
    [SerializeField] Sprite SurpriseBackImage;
    [SerializeField] Sprite ThinkingBackImage;
    [SerializeField] Sprite FearBackImage;

    private float paddingHeight = 90f;
    private float paddingWidth = 50f;
    private float maxWidth = 250f;
    private float blowingWidth = 250f;
    private List<TalkMessage> messageList = new List<TalkMessage>();
    private Coroutine fadeCoroutine;
    private Coroutine messageCoroutine;
    private Coroutine typingCoroutine;

    private void OnDisable()
    {
        if (messageCoroutine != null)
        {
            messageList.Clear(); // メッセージリストをクリア
            messageText.SetText(""); // テキストをクリア   
            ResizePlate();
            StopCoroutine(messageCoroutine);
            messageCoroutine = null;
        }
    }

    private void ClearMessage()
    {
        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
            messageCoroutine = null;
        }
        messageText.SetText(""); // テキストをクリア
        messageList.Clear(); // メッセージリストをクリア
        ResizePlate();
    }

    public IEnumerator AddMessage(TalkMessage talkMessage)
    {
        // 前のTypeDialogが動いていたら停止
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        // Textをクリア
        messageText.text = "";
        messageList.Clear(); // 既存のメッセージをクリア
        SetPanel(talkMessage.messagePanelType); // パネルの種類を設定

        // TypeDialogの開始（Coroutineとして保持）
        typingCoroutine = StartCoroutine(TypeDialog(talkMessage.message));

        // TypeDialog終了まで待つ
        yield return typingCoroutine;
        typingCoroutine = null;

        float waitTime = Mathf.Clamp(talkMessage.message.Length * 0.2f, 2f, 6f); // 最小2秒、最大6秒
        yield return new WaitForSeconds(waitTime);

        messageCoroutine = null;
        gameObject.SetActive(false); // 表示終了後に非アクティブにする
    }

    public IEnumerator AddMessageList(TalkMessage talkMessage)
    {
        messageList.Add(talkMessage);
        if (messageCoroutine == null)
        {
            if (gameObject.activeSelf)
            {
                yield return messageCoroutine = StartCoroutine(TypeMessageList());
            }
        }
        else
        {
            // すでに動いてるなら終わるのを待つ
            while (messageCoroutine != null)
            {
                yield return null;
            }
        }
    }

    private IEnumerator TypeMessageList()
    {
        while (messageList.Count > 0)
        {
            string message = messageList[0].message; // 先頭のメッセージを取得
            SetPanel(messageList[0].messagePanelType); // パネルの種類を設定
            yield return TypeDialog(message);
            // messageの文字数によって待ち時間を変更する
            float waitTime = Mathf.Clamp(message.Length * 0.2f, 2f, 6f); // 最小1秒、最大5秒
            yield return new WaitForSeconds(waitTime);
            messageList.RemoveAt(0); // タイプし終わったメッセージを削除
        }
        messageCoroutine = null; // すべてのメッセージが終了したら、コルーチンの参照をクリア
        transform.gameObject.SetActive(false); // すべてのメッセージが終了したら、オブジェクトを非アクティブにする
    }

    private void SetPanel(MessagePanelType panelType)
    {
        switch (panelType)
        {
            case MessagePanelType.Default:
                panelImage.sprite = DefaultBackImage;
                break;
            case MessagePanelType.Surprise:
                panelImage.sprite = SurpriseBackImage;
                break;
            case MessagePanelType.Thinking:
                panelImage.sprite = ThinkingBackImage;
                break;
            case MessagePanelType.Fear:
                panelImage.sprite = FearBackImage;
                break;
            default:
                panelImage.sprite = DefaultBackImage;
                break;
        }
    }

    public IEnumerator TypeDialog(string line)
    {
        messageText.SetText("");
        // lineの文字数によって横幅を変える。（20文字以上ならmaxWidthでそれ以内なら。その時のサイズ）
        if (line.Length > 15)
        {
            blowingWidth = maxWidth + paddingWidth;
        }
        else
        {
            blowingWidth = line.Length * 30f + paddingWidth;
        }
        foreach (char letter in line)
        {
            messageText.text += letter;
            ResizePlate();
            yield return new WaitForSeconds(0.03f);
        }
    }

    private void ResizePlate()
    {
        if (messageText == null || backRectTransform == null)
        {
            Debug.LogError("messageText または backRectTransform が null");
            return;
        }

        // TextMeshProのTextにレイアウトを再計算させる
        messageText.ForceMeshUpdate();

        // 横幅を最大値で制限
        // float newWidth = Mathf.Min(messageText.preferredWidth, maxWidth) + paddingWidth;
        float newHeight = messageText.preferredHeight + paddingHeight;
        backRectTransform.sizeDelta = new Vector2(blowingWidth, newHeight);
    }
}
