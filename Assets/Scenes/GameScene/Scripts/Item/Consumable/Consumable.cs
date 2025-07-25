using UnityEngine;

[System.Serializable]
public class Consumable : Item
{
    [SerializeField] ConsumableBase _base;

    [field: SerializeField]
    public int UsableCount { get; private set; }

    public override ItemType itemType => ItemType.Consumable;
    public override ItemBase Base => _base;
    public ConsumableBase ConsumableBase => _base;

    public Consumable(ConsumableBase baseData)
    {
        _base = baseData;
        UsableCount = baseData.UsableCount;
    }

    public void Initialize()
    {
        UsableCount = _base?.UsableCount ?? 1;
    }
}
