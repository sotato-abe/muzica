using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EquipmentInfo : MonoBehaviour
{
    [SerializeField] TargetIcon targetIcon;
    [SerializeField] GameObject enchantList;
    [SerializeField] GameObject counterList;
    [SerializeField] EnchantIcon enchantIconPrefab;
    [SerializeField] AttackCounter attackCounterPrefab;

    private Equipment equipment = new Equipment(null); // 初期化のためにnullを渡す
    private TargetType TargetType = TargetType.Individual; // 初期値を設定
    private List<EnergyCount> EnergyAttackList = new List<EnergyCount>();
    private List<Enchant> EnchantList = new List<Enchant>();

    public void SetInfo(Equipment equipment)
    {
        gameObject.SetActive(true);

        this.equipment = equipment;
        TargetType = equipment.EquipmentBase.TargetType;
        EnchantList = new List<Enchant>();
        foreach (var enchant in equipment.EquipmentBase.EnchantList)
        {
            EnchantList.Add(new Enchant(enchant)); // コピーコンストラクタを使う（後述）
        }

        EnergyAttackList = new List<EnergyCount>();
        foreach (var attack in equipment.EquipmentBase.EnergyAttackList)
        {
            // LIFEアタックのときPlayerのPOWを加算する
            if (attack.type == EnergyType.Life || attack.isRecovery == false)
            {
                EnergyCount colAttack = new EnergyCount(attack);
                colAttack.val += PlayerController.Instance.PlayerCharacter.ColPower;
                EnergyAttackList.Add(colAttack);
                continue;
            }
            if (attack.type == EnergyType.Battery || attack.isRecovery == false)
            {
                EnergyCount colAttack = new EnergyCount(attack);
                colAttack.val += PlayerController.Instance.PlayerCharacter.ColTechnique;
                EnergyAttackList.Add(colAttack);
                continue;
            }
            EnergyAttackList.Add(new EnergyCount(attack)); // コピーコンストラクタを使う（後述）
        }

        SetTargetIcon();
        SetEnchants();
        SetAttacks();
    }

    private void SetTargetIcon()
    {
        targetIcon.SetTargetType(TargetType);
    }

    private void SetEnchants()
    {

        foreach (Transform child in enchantList.transform)
        {
            Destroy(child.gameObject);
        }

        // エンチャントを表示する処理
        foreach (var enchant in EnchantList)
        {
            EnchantIcon newIcon = Instantiate(enchantIconPrefab, enchantList.transform);
            newIcon.SetEnchant(enchant);
        }
    }

    private void SetAttacks()
    {

        // 既存のカウンターを削除
        foreach (Transform child in counterList.transform)
        {
            Destroy(child.gameObject);
        }
        // 装備のコストを表示する処理
        foreach (var attack in EnergyAttackList)
        {
            AttackCounter attackCounter = Instantiate(attackCounterPrefab, counterList.transform);
            attackCounter.SetCounter(attack);
        }
    }

    public void CharacterStatusUpdate(Character character)
    {
        int ColPower = character.ColPower;

    }

    public void CommandUpdate(Command command)
    {
        if (command == null || command.Base == null)
        {
            Debug.LogWarning("Command base is null, cannot update info.");
            return;
        }

        // コマンドの情報を更新する処理
        MergeEnchantList(command.EnchantList);
        MergeAttackList(command.EnergyAttackList);
        UpdateInfo();
    }

    private void MergeEnchantList(List<Enchant> newEnchants)
    {
        // EnchantListに新しいエンチャントを追加する同じタイプはまとめる
        foreach (var newEnchant in newEnchants)
        {
            bool found = false;
            foreach (var existingEnchant in EnchantList)
            {
                if (existingEnchant.Type == newEnchant.Type)
                {
                    existingEnchant.Val += newEnchant.Val; // 値を加算
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                EnchantList.Add(new Enchant(newEnchant)); // コピーコンストラクタを使用して新しいインスタンスを作成
            }
        }
    }

    private void MergeAttackList(List<EnergyCount> newAttacks)
    {
        // EnergyAttackListに新しい攻撃を追加する同じタイプはまとめる
        foreach (var newAttack in newAttacks)
        {
            bool found = false;
            foreach (var existingAttack in EnergyAttackList)
            {
                if (existingAttack.type == newAttack.type)
                {
                    existingAttack.val += newAttack.val; // 値を加算
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                EnergyAttackList.Add(new EnergyCount(newAttack)); // コピーコンストラクタを使用して新しいインスタンスを作成
            }
        }
    }

    public TotalAttackCount GetTotalCount()
    {
        TotalAttackCount totalCount = new TotalAttackCount
        {
            TargetType = TargetType,
            EnergyAttackList = new List<EnergyCount>(EnergyAttackList),
            EnchantList = new List<Enchant>(EnchantList)
        };
        return totalCount;
    }

    private void UpdateInfo()
    {
        SetTargetIcon();
        SetEnchants();
        SetAttacks();
    }
}