using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnchant", menuName = "Enchant/Enchant Data")]
public class EnchantData : ScriptableObject
{
    public EnchantType enchantType;
    [SerializeField] public string enchantName;
    [SerializeField] public Sprite icon;
    [TextArea] public string description;
}
