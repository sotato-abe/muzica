using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnchantPrefab : MonoBehaviour
{
    [SerializeField] Image enchantIcon;
    [SerializeField] TextMeshProUGUI valText;

    [Header("Buff Icons")]
    [SerializeField] Sprite AccelerationIcon;
    [SerializeField] Sprite LuckyIcon;
    [SerializeField] Sprite GazeIcon;
    [SerializeField] Sprite AnalysisIcon;
    [SerializeField] Sprite PowerIcon;
    [SerializeField] Sprite AdrenalinIcon;
    [SerializeField] Sprite GuardIcon;
    [SerializeField] Sprite SolidIcon;
    [SerializeField] Sprite CuringIcon;
    [SerializeField] Sprite SplinterIcon;
    [SerializeField] Sprite ReflectionIcon;
    [SerializeField] Sprite CamouflageIcon;
    [SerializeField] Sprite ClearIcon;

    [Header("DeBuff Icons")]
    [SerializeField] Sprite SlowIcon;
    [SerializeField] Sprite UnLuckyIcon;
    [SerializeField] Sprite FatigueIcon;
    [SerializeField] Sprite LockIcon;
    [SerializeField] Sprite CipherIcon;
    [SerializeField] Sprite BugIcon;
    [SerializeField] Sprite AtrophyIcon;
    [SerializeField] Sprite BlindIcon;
    [SerializeField] Sprite ParalysisIcon;
    [SerializeField] Sprite CrackIcon;
    [SerializeField] Sprite PoisonIcon;
    [SerializeField] Sprite LeakageIcon;
    [SerializeField] Sprite HurtIcon;
    [SerializeField] Sprite SleepIcon;

    public void SetEnchant(Enchant enchant)
    {
        SetIcon(enchant.Type);
        valText.text = enchant.Val.ToString();
    }

    private void SetIcon(EnchantType enchantType)
    {
        switch (enchantType)
        {
            case EnchantType.Acceleration:
                enchantIcon.sprite = AccelerationIcon;
                break;
            case EnchantType.Lucky:
                enchantIcon.sprite = LuckyIcon;
                break;
            case EnchantType.Gaze:
                enchantIcon.sprite = GazeIcon;
                break;
            case EnchantType.Analysis:
                enchantIcon.sprite = AnalysisIcon;
                break;
            case EnchantType.Power:
                enchantIcon.sprite = PowerIcon;
                break;
            case EnchantType.Adrenalin:
                enchantIcon.sprite = AdrenalinIcon;
                break;
            case EnchantType.Guard:
                enchantIcon.sprite = GuardIcon;
                break;
            case EnchantType.Solid:
                enchantIcon.sprite = SolidIcon;
                break;
            case EnchantType.Curing:
                enchantIcon.sprite = CuringIcon;
                break;
            case EnchantType.Splinter:
                enchantIcon.sprite = SplinterIcon;
                break;
            case EnchantType.Reflection:
                enchantIcon.sprite = ReflectionIcon;
                break;
            case EnchantType.Camouflage:
                enchantIcon.sprite = CamouflageIcon;
                break;
            case EnchantType.Clear:
                enchantIcon.sprite = ClearIcon;
                break;
            case EnchantType.Slow:
                enchantIcon.sprite = SlowIcon;
                break;
            case EnchantType.UnLucky:
                enchantIcon.sprite = UnLuckyIcon;
                break;
            case EnchantType.Fatigue:
                enchantIcon.sprite = FatigueIcon;
                break;
            case EnchantType.Lock:
                enchantIcon.sprite = LockIcon;
                break;
            case EnchantType.Cipher:
                enchantIcon.sprite = CipherIcon;
                break;
            case EnchantType.Bug:
                enchantIcon.sprite = BugIcon;
                break;
            case EnchantType.Atrophy:
                enchantIcon.sprite = AtrophyIcon;
                break;
            case EnchantType.Blind:
                enchantIcon.sprite = BlindIcon;
                break;
            case EnchantType.Paralysis:
                enchantIcon.sprite = ParalysisIcon;
                break;
            case EnchantType.Crack:
                enchantIcon.sprite = CrackIcon;
                break;
            case EnchantType.Poison:
                enchantIcon.sprite = PoisonIcon;
                break;
            case EnchantType.Leakage:
                enchantIcon.sprite = LeakageIcon;
                break;
            case EnchantType.Hurt:
                enchantIcon.sprite = HurtIcon;
                break;
            case EnchantType.Sleep:
                enchantIcon.sprite = SleepIcon;
                break;
            default:
                Debug.LogError("Unknown EnchantType");
                break;
        }
    }
}
