using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyGauge : MonoBehaviour
{
    [SerializeField] EnergyBar lifeBar;
    [SerializeField] EnergyBar batteryBar;
    [SerializeField] EnergyBar soulBar;

    public void SetLifeGauge(int maxEnergy, int currentEnergy)
    {
        if (maxEnergy <= 0)
        {
            Debug.LogError("Max energy must be greater than zero.");
            return;
        }
        lifeBar.SetEnergy(maxEnergy, currentEnergy);
    }

    public void SetBatteryGauge(int maxEnergy, int currentEnergy)
    {
        if (maxEnergy <= 0)
        {
            Debug.LogError("Max energy must be greater than zero.");
            return;
        }
        batteryBar.SetEnergy(maxEnergy, currentEnergy);
    }
    public void SetSoulGauge(int maxEnergy, int currentEnergy)
    {
        if (maxEnergy <= 0)
        {
            Debug.LogError("Max energy must be greater than zero.");
            return;
        }
        soulBar.SetEnergy(maxEnergy, currentEnergy);
    }

    public IEnumerator SetLifeValueCoroutine(int value)
    {
        yield return lifeBar.SetValueCoroutine(value);
    }

    public IEnumerator SetBatteryValueCoroutine(int value)
    {
        yield return batteryBar.SetValueCoroutine(value);
    }

    public IEnumerator SetSoulValueCoroutine(int value)
    {
        yield return soulBar.SetValueCoroutine(value);
    }
}

