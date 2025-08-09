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
            ColStatus();
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

    public bool AddCommandToStorage(Command command)
    {
        if (StorageList.Count < ColStorage)
        {
            StorageList.Add(command.Clone());
            return true;
        }

        Debug.Log("ストレージがいっぱいです。");
        return false;
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
