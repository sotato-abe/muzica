using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

[System.Serializable]
public class EnemyCharacter : Character
{
    public EnemyCharacter(CharacterBase baseData) : base(baseData) { }
    private Dictionary<StatusType, int> statusDictionary = new Dictionary<StatusType, int>();

    public void LevelUp(int increase)
    {
        statusDictionary = new Dictionary<StatusType, int>();

        statusDictionary[StatusType.LIFE] = MaxLife / 10;
        statusDictionary[StatusType.BTRY] = MaxBattery / 5;
        statusDictionary[StatusType.POW] = Power;
        statusDictionary[StatusType.DEF] = Defense;
        statusDictionary[StatusType.TEC] = Technique;
        statusDictionary[StatusType.SPD] = Speed;
        statusDictionary[StatusType.LUK] = Luck;

        for (int i = 0; i < increase; i++)
        {
            StatusType targetStatus = WeightedRandomChoice();

            switch (targetStatus)
            {
                case StatusType.LIFE:
                    MaxLife += 10;
                    Life = MaxLife;
                    break;
                case StatusType.BTRY:
                    MaxBattery += 5;
                    Battery = MaxBattery;
                    break;
                case StatusType.POW:
                    Power += 1;
                    break;
                case StatusType.DEF:
                    Defense += 1;
                    break;
                case StatusType.TEC:
                    Technique += 1;
                    break;
                case StatusType.SPD:
                    Speed += 1;
                    break;
                case StatusType.LUK:
                    Luck += 1;
                    break;
            }
        }
        Level = increase;
    }

    private StatusType WeightedRandomChoice()
    {
        // ステータスが高いものほど選ばれやすいように確率を設定
        // １番: 40%, ２番: 25%, ３番: 15%, ４番: 10%, ５番: 6%, ６番: 4%
        float[] probabilities = { 0.4f, 0.25f, 0.15f, 0.10f, 0.06f, 0.04f };

        float r = Random.value;
        float cumulative = 0f;

        int index = 0;
        foreach (var key in statusDictionary.Keys)
        {
            if (index >= probabilities.Length) break; // 安全対策
            cumulative += probabilities[index];
            if (r < cumulative)
            {
                return key;
            }
            index++;
        }

        return statusDictionary.Keys.First();
    }


    public TotalAttackCount EnemyAttack()
    {
        Equipment activeEquipment = GetRandomActiveEquipment();
        TotalAttackCount totalAttackCount = new TotalAttackCount();
        if (activeEquipment != null)
        {
            // アクティブな装備がある場合、攻撃を実行
            // Lifeへのアタックがある時エネミーのColPowerを加算する
            foreach (EnergyCount original in activeEquipment.EquipmentBase.EnergyAttackList)
            {
                EnergyCount attack = new EnergyCount(original);
                if (attack.type == EnergyType.Life && attack.isRecovery == false)
                {
                    attack.val += ColPower;
                }
                if (attack.type == EnergyType.Battery && attack.isRecovery == false)
                {
                    attack.val += ColTechnique;
                }
                totalAttackCount.EnergyAttackList.Add(attack);
            }

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
