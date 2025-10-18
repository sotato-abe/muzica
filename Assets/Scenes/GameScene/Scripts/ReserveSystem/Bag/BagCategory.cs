using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagCategory : MonoBehaviour
{
    [SerializeField] GameObject equipmentButton;
    [SerializeField] GameObject pocketButton;
    [SerializeField] GameObject backPanel;

    public delegate void ChangeWindowDelegate(bool isEquipment);
    public event ChangeWindowDelegate OnChangeWindow;


    public void SetEquipmentButtonActive()
    {
        // backPanelの位置をequipmentButtonの位置に合わせる
        StartCoroutine(SlidePanel(equipmentButton));
        // ウィンドウの切り替えイベントを発火
        OnChangeWindow?.Invoke(true);
    }

    public void SetPocketButtonActive()
    {
        // backPanelの位置をpocketButtonの位置に合わせる
        StartCoroutine(SlidePanel(pocketButton));
        OnChangeWindow?.Invoke(false);
    }

    public void SwitchActiveButton()
    {
        if (backPanel.transform.position == equipmentButton.transform.position)
        {
            SetPocketButtonActive();
        }
        else
        {
            SetEquipmentButtonActive();
        }
    }

    private IEnumerator SlidePanel(GameObject targetButton)
    {
        Vector3 targetPosition = targetButton.transform.position;
        Vector3 startPosition = backPanel.transform.position;
        float duration = 0.2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // イージング（滑らか補間）
            float easedT = Mathf.SmoothStep(0f, 1f, t);

            backPanel.transform.position = Vector3.Lerp(startPosition, targetPosition, easedT);
            yield return null;
        }

        // 最終位置を明示的に補正（ズレ防止）
        backPanel.transform.position = targetPosition;
    }
}