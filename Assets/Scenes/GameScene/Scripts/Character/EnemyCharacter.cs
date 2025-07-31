using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class EnemyCharacter : Character
{

    public TotalAttackCount EnemyAttack()
    {
        Equipment activeEquipment = GetRandomActiveEquipment();
        TotalAttackCount totalAttackCount = new TotalAttackCount();
        if (activeEquipment != null)
        {
            // アクティブな装備がある場合、攻撃を実行
            // アタック値を設定
            totalAttackCount.EnergyAttackList.AddRange(activeEquipment.EquipmentBase.EnergyAttackList);
            totalAttackCount.EnchantList.AddRange(activeEquipment.EquipmentBase.EnchantList);

            //コストを消費
            foreach (EnergyCost energyCost in activeEquipment.EquipmentBase.EnergyCostList)
            {
                switch (energyCost.type)
                {
                    case EnergyType.Life:
                        Life -= energyCost.val;
                        break;
                    case EnergyType.Battery:
                        Battery -= energyCost.val;
                        break;
                    case EnergyType.Soul:
                        Soul -= energyCost.val;
                        break;
                }
            }
        }
        else
        {
            Debug.Log("No active equipment available for attack.");
        }

        return totalAttackCount;
    }

    private Equipment GetRandomActiveEquipment()
    {
        int equipmentCount = EquipmentList.Count;
        List<Equipment> activeEquipmentList = new List<Equipment>(); // 敵の装備リストを取得

        foreach (Equipment equipment in EquipmentList)
        {
            // EquipmentListの中からエナジーコストが自身のエナジーを超えない装備を取得する。
            if (equipment.EquipmentBase.EnergyCostList.Count == 0)
            {
                activeEquipmentList.Add(equipment);
                continue;
            }

            // エナジーコストがある場合は、エナジーコストのエナジータイプが自身のエナジーを超えない装備を取得する。
            bool canUse = true;
            foreach (EnergyCost energyCost in equipment.EquipmentBase.EnergyCostList)
            {
                if (energyCost.type == EnergyType.Life && Life <= energyCost.val)
                {
                    canUse = false;
                    break;
                }
                if (energyCost.type == EnergyType.Battery && Battery <= energyCost.val)
                {
                    canUse = false;
                    break;
                }
                if (energyCost.type == EnergyType.Soul && Soul <= energyCost.val)
                {
                    canUse = false;
                    break;
                }
            }
            if (canUse)
            {
                activeEquipmentList.Add(equipment);
            }
        }

        if (activeEquipmentList.Count == 0)
        {
            return null; // 使用可能な装備がない場合はnullを返す
        }
        int randomIndex = Random.Range(0, activeEquipmentList.Count);
        return activeEquipmentList[randomIndex]; // ランダムに装備を選択して返す
    }
}
