using UnityEngine;

[System.Serializable]
public class Consumable : Item
{
    [field: SerializeField]
    public int UsableCount { get; set; }

    // ConsumableBase にキャストしたプロパティ
    public ConsumableBase ConsumableBase => _base as ConsumableBase;

    public override ItemBase Base => _base;

    // コンストラクタで base._base を設定
    public Consumable(ConsumableBase baseData) : base(baseData)
    {
        UsableCount = baseData.UsableCount;
    }

    public bool UseConsumable()
    {
        UsableCount = Mathf.Max(UsableCount - 1, 0);
        return UsableCount > 0;
    }

    public override Item Clone()
    {
        var copy = new Consumable(ConsumableBase);
        copy.UsableCount = this.UsableCount;
        return copy;
    }
}
