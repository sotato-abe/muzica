using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PlayerCharacter : Character
{
    private const int EQUIPMENT_COUNT = 2;
    // public int SkillPoint { get; set; } = 0;

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

    public void EnergyUp(EnergyType type)
    {
        if (SkillPoint > 0)
        {
            switch (type)
            {
                case EnergyType.Life:
                    MaxLife += 10;
                    break;
                case EnergyType.Battery:
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

    public bool AddItemToPocket(Consumable consumable)
    {
        if (PocketList.Count < ColPocket)
        {
            PocketList.Add(consumable);
            return true;
        }

        Debug.Log("ポケットがいっぱいです。");
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

    public void AddExp(int exp)
    {
        if (exp <= 0) return;

        Exp += exp;

        while (Exp >= 100)
        {
            Level++;
            SkillPoint++; // レベルアップ時にスキルポイントを追加
            Exp -= 100;
        }
    }

    public void AddCoin(int coin)
    {
        if (coin <= 0) return;

        Coin += coin;
    }
}
