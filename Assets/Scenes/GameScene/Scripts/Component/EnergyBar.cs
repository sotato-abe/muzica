using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] EnergyType energyType;
    [SerializeField] TextMeshProUGUI counterText;
    [SerializeField] Image barImage;
    [SerializeField] DiffCounter diffCounterPrefab;
    [SerializeField] GuardWindow guardWindow;

    int maxEnergy = 100;
    int currentEnergy = 0;
    Coroutine fillCoroutine;

    public void SetEnergy(int maxEnergy, int currentEnergy)
    {
        if (maxEnergy <= 0)
        {
            Debug.LogError("Max energy must be greater than zero.");
            return;
        }

        this.maxEnergy = maxEnergy;
        this.currentEnergy = currentEnergy;

        float fillAmount = (float)currentEnergy / maxEnergy;
        barImage.fillAmount = Mathf.Clamp01(fillAmount);
        counterText.text = $"{currentEnergy}/{maxEnergy}";
    }

    public void SetGuard(int guard)
    {
        if (guardWindow != null)
        {
            guardWindow.SetGuardCounter(guard);
            guardWindow.gameObject.SetActive(guard > 0);
        }
    }

    public IEnumerator SetValueCoroutine(int value, int maxValue)
    {
        if (value < 0) value = 0;
        if (maxValue > 0) maxEnergy = maxValue;

        value = Mathf.Min(value, maxEnergy);
        ShowDiffCounter(value - currentEnergy);
        currentEnergy = value;

        float targetFill = (float)currentEnergy / maxEnergy;
        counterText.text = $"{currentEnergy}/{maxEnergy}";

        if (!gameObject.activeInHierarchy)
        {
            barImage.fillAmount = targetFill;
            yield break;
        }

        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }

        yield return fillCoroutine = StartCoroutine(SmoothFillCoroutine(targetFill));
    }


    public IEnumerator UpdateValueCoroutine(int value, int guard, int takeValue)
    {
        if (value < 0) value = 0;
        if (guard > 0)
            SetGuard(guard);
    
        ShowDiffCounter(takeValue);
        currentEnergy = value;

        float targetFill = (float)currentEnergy / maxEnergy;
        counterText.text = $"{currentEnergy}/{maxEnergy}";

        if (!gameObject.activeInHierarchy)
        {
            barImage.fillAmount = targetFill;
            yield break;
        }

        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }

        yield return fillCoroutine = StartCoroutine(SmoothFillCoroutine(targetFill));
    }

    private void ShowDiffCounter(int value)
    {
        if (value == 0) return;

        DiffCounter diffCounter = Instantiate(diffCounterPrefab, transform);
        diffCounter.SetDiffCounter(energyType, value);
        RectTransform diffRect = diffCounter.GetComponent<RectTransform>();
        diffCounter.transform.SetAsFirstSibling(); // 最前面に表示
    }


    private IEnumerator SmoothFillCoroutine(float targetFill)
    {
        float duration = 0.3f;
        float startFill = barImage.fillAmount;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            barImage.fillAmount = Mathf.Lerp(startFill, targetFill, t);
            yield return null;
        }

        barImage.fillAmount = targetFill;
    }
}
