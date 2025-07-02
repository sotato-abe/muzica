using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquipmentBase", menuName = "Item/EquipmentBase")]
public class EquipmentBase : ItemBase
{
    [SerializeField] EquipmentType equipmentType; // 装備の種類
    [SerializeField] int attackPower; // 攻撃力（武器やツールの場合）
    [SerializeField] int defensePower; // 防御力（防具の場合）

    public EquipmentType EquipmentType { get => equipmentType; }
    public int AttackPower { get => attackPower; }
    public int DefensePower { get => defensePower; }
}
