using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PlayerCharacter : Character
{
    private const int EQUIPMENT_COUNT = 2;
    public int SkillPoint { get; set; } = 0;

    public override void Init()
    {
        base.Init();
    }

    public void AcquisitionExp(int exp)
    {
        Exp += exp;

        int level = Exp / 100;
        if (level > 0)
        {
            Exp -= level * 100;
            Level += level;
            SkillPoint += level;
        }
    }

    public void EnegyUp(EnegyType type)
    {
        if (SkillPoint > 0)
        {
            switch (type)
            {
                case EnegyType.Life:
                    MaxLife += 10;
                    break;
                case EnegyType.Battery:
                    MaxBattery += 5;
                    break;
            }
            CoLStatus();
            SkillPoint -= 1;
        }
        else
        {
            Debug.Log("スキルポイントが足りません。");
        }
        Debug.Log($"スキルポイント: {ColLife},{MaxLife}/{Life}");
    }

    public void StatusUp(StatusType type)
    {
        if (SkillPoint > 0)
        {
            switch (type)
            {
                case StatusType.POW:
                    Power += 1;
                    break;
                case StatusType.TEC:
                    Technique += 1;
                    break;
                case StatusType.DEF:
                    Defense += 1;
                    break;
                case StatusType.SPD:
                    Speed += 1;
                    break;
                case StatusType.LUK:
                    Luck += 1;
                    break;
                case StatusType.MMR:
                    Memory += 1;
                    break;
                case StatusType.STG:
                    Storage += 1;
                    break;
                case StatusType.POC:
                    Pocket += 1;
                    break;
            }
            SkillPoint -= 1;
            CoLStatus();
        }
        else
        {
            Debug.Log("スキルポイントが足りません。");
        }
    }

    public bool AddItemToBag(Item item)
    {
        if (BagItemList.Count < Bag)
        {
            BagItemList.Add(item);
            return true;
        }

        Debug.Log("バッグがいっぱいです。");
        return false;
    }

    public bool EquipItem(Equipment equipment)
    {
        EquipmentList.Add(equipment);
        // 3つ以上ある場合は一番頭の装備を外しBagItemListに追加
        if (EquipmentList.Count >= EQUIPMENT_COUNT)
        {
            Equipment removedEquipment = EquipmentList[0];
            EquipmentList.RemoveAt(0);
            BagItemList.Add(removedEquipment);
        }
        return true;
    }

    public bool RemoveBagItem(Item item)
    {
        if (item == null || !BagItemList.Contains(item))
        {
            Debug.Log("アイテムが見つかりません。");
            return false;
        }

        BagItemList.Remove(item);
        return true;
    }
}
