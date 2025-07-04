using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PlayerCharacter : Character
{
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

    public bool AddItem(Item item)
    {
        switch (item)
        {
            case Consumable consumable:
                return TryAddToBag(consumable);
            // if (PocketList.Count < Pocket)
            // {
            //     PocketList.Add(consumable);
            //     return true;
            // }
            // else
            // {
            // return TryAddToBag(consumable);
            // }

            case Equipment equipment:
                return TryAddToBag(equipment);

            case Treasure treasure:
                return TryAddToBag(treasure);

            default:
                Debug.Log("Unknown item type.");
                return false;
        }
    }

    public bool RemoveItem(Item item)
    {
        if (item == null || !BagItemList.Contains(item))
        {
            Debug.Log("アイテムが見つかりません。");
            return false;
        }

        BagItemList.Remove(item);
        return true;
    }

    private bool TryAddToBag(Item item)
    {
        if (BagItemList.Count < Bag)
        {
            BagItemList.Add(item);
            return true;
        }

        Debug.Log("バッグがいっぱいです。");
        return false;
    }
}
