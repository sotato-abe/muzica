using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class SelectWindow : Window
{
    [SerializeField] public List<SelectElement> selectElements;
    [SerializeField] Image backgroundImage;

    private int currentIndex = 0;
    private bool isCursolActive = false;
    private float inputCooldownTime = 0f;
    private const float INPUT_COOLDOWN_DURATION = 0.1f; // 100msの入力クールダウン（入力制限）

    public delegate void ChangeTargetDelegate(int index);
    public event ChangeTargetDelegate OnChangeTarget;
    public UnityAction OnEnterTargetWindow;
    public UnityAction OnExitWindow;

    private Color activeBackPanelColor = new Color(13f / 255f, 103f / 255f, 116f / 255f, 255f / 255f);
    private Color inactiveBackPanelColor = new Color(0f, 0f, 0f, 100f / 255f);


    public virtual void Start()
    {
        ChangeTargetCursol();
    }

    private void Update()
    {
        if (!isCursolActive) return;

        // 入力クールダウンを更新
        if (inputCooldownTime > 0f)
        {
            inputCooldownTime -= Time.deltaTime;
            return; // クールダウン中は入力を無視
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveCursol(1);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveCursol(-1);
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            EnterTargetWindow();
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ExitWindow();
        }
    }

    // ウィンドウを開くときに使用
    public override void WindowOpen()
    {
        base.WindowOpen();
        isCursolActive = true;
    }

    // ウィンドウを閉じるときに使用
    public override void WindowClose()
    {
        base.WindowClose();
        isCursolActive = false;
    }

    // 下位パネルに行くときに使用
    public virtual void EnterTargetWindow()
    {
        ChangeActiveWindow(false);
        OnEnterTargetWindow?.Invoke();
    }

    //　上位パネルに行くときに使用
    public virtual void ExitWindow()
    {
        ChangeActiveWindow(false);
        OnExitWindow?.Invoke();
    }

    public void ChangeActiveWindow(bool isActive)
    {
        if (isActive)
        {
            // 即座にアクティブ化する場合
            isCursolActive = true;
            backgroundImage.color = activeBackPanelColor;
            // ChangeTargetCursol();
            // ウィンドウがアクティブになった直後は入力をブロック
            inputCooldownTime = INPUT_COOLDOWN_DURATION;
        }
        else
        {
            isCursolActive = false;
            backgroundImage.color = inactiveBackPanelColor;
        }
    }

    private void MoveCursol(int direction)
    {
        selectElements[currentIndex].SetActiveCursol(false);
        currentIndex += direction;

        if (currentIndex < 0)
        {
            currentIndex = selectElements.Count - 1;
        }
        else if (currentIndex >= selectElements.Count)
        {
            currentIndex = 0;
        }
        ChangeTargetCursol();
    }

    private void ChangeTargetCursol()
    {
        SoundSystem.Instance.PlaySE(SeType.Select);
        for (int i = 0; i < selectElements.Count; i++)
        {
            selectElements[i].SetIndex(i);
            selectElements[i].SetActiveCursol(i == currentIndex);
        }
        OnChangeTarget?.Invoke(currentIndex);
        TargetElement(currentIndex);

        RectTransform contentRect = selectElements[0].transform.parent.GetComponent<RectTransform>();
        RectTransform targetRect = selectElements[currentIndex].GetComponent<RectTransform>();
        float contentHeight = contentRect.rect.height;
        float targetPosY = Mathf.Abs(targetRect.anchoredPosition.y);
        float targetHeight = targetRect.rect.height;
        float viewportHeight = contentRect.parent.GetComponent<RectTransform>().rect.height;
        float scrollPosY = contentRect.anchoredPosition.y;
        if (targetPosY - scrollPosY < 0)
        {
            scrollPosY = targetPosY;
        }
        else if (targetPosY + targetHeight - scrollPosY > viewportHeight)
        {
            scrollPosY = targetPosY + targetHeight - viewportHeight;
        }
        contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, scrollPosY);
    }

    public virtual void TargetElement(int index)
    {
        // 各Windowでオーバーライドして使用
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }
}
