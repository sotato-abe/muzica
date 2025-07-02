using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consumable")]
public class ConsumableBase : ItemBase
{
    [SerializeField] ConsumableType consumableType; // 消耗品の種類
    [SerializeField] int recoveryAmount; // 回復量（ポーションや食べ物の場合）
    [SerializeField] int damageAmount; // ダメージ量（爆弾の場合）

    public ConsumableType ConsumableType { get => consumableType; }
    public int RecoveryAmount { get => recoveryAmount; }
    public int DamageAmount { get => damageAmount; }

}
