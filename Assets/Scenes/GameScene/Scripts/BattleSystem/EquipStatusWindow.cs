using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipStatusWindow : MonoBehaviour
{
    [SerializeField] Image statusImage;
    [SerializeField] Sprite energyOutImage;
    [SerializeField] Sprite stopImage;
    [SerializeField] Sprite brokenImage;
    [SerializeField] Sprite mysteryImage;
    [SerializeField] Sprite emptyImage;

    private EquipStatusType currentStatus = EquipStatusType.Empty;

    public void SetStatus(EquipStatusType status)
    {
        currentStatus = status;
        UpdateStatusImage();
    }

    private void UpdateStatusImage()
    {
        switch (currentStatus)
        {
            case EquipStatusType.Active:
                statusImage.sprite = null; // アクティブ状態の場合は画像を非表示にする
                break;
            case EquipStatusType.EnergyOut:
                statusImage.sprite = energyOutImage;
                break;
            case EquipStatusType.Stop:
                statusImage.sprite = stopImage;
                break;
            case EquipStatusType.Broken:
                statusImage.sprite = brokenImage;
                break;
            case EquipStatusType.Mystery:
                statusImage.sprite = mysteryImage;
                break;
            case EquipStatusType.Empty:
                statusImage.sprite = emptyImage;
                break;
        }
    }
}
