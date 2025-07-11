using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI counterText;
    [SerializeField] Image barImage;

    int maxEnergy = 100; // Default max energy, can be adjusted as needed
    int currentEnergy = 0; // Current energy, can be set externally

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

    public void SetValue(int value)
    {
        if (value < 0)
        {
            Debug.LogError("Energy value cannot be negative.");
            value = 0; // Ensure value is not negative
        }

        this.currentEnergy = value;
        float fillAmount = (float)currentEnergy / maxEnergy;
        barImage.fillAmount = Mathf.Clamp01(fillAmount); // Assuming max value is 100
        counterText.text = $"{currentEnergy}/{maxEnergy}";
    }
}

