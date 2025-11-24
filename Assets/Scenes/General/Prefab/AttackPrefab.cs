using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AttackPrefab : MonoBehaviour
{
    [SerializeField] Image attackIcon;
    [SerializeField] TextMeshProUGUI valText;
    [SerializeField] TextMeshProUGUI timesText;

    [SerializeField] Sprite SoloLifeUpIcon;
    [SerializeField] Sprite GroupLifeUpIcon;
    [SerializeField] Sprite SoloBatteryUpIcon;
    [SerializeField] Sprite GroupBatteryUpIcon;
    [SerializeField] Sprite SoloSoulUpIcon;
    [SerializeField] Sprite GroupSoulUpIcon;
    [SerializeField] Sprite SoloGuardIcon;
    [SerializeField] Sprite GroupGuardIcon;
    [SerializeField] Sprite SoloSecurityIcon;
    [SerializeField] Sprite GroupSecurityIcon;
    [SerializeField] Sprite SoloBuffIcon;
    [SerializeField] Sprite GroupBuffIcon;

    [SerializeField] Sprite SoloLifeDownIcon;
    [SerializeField] Sprite GroupLifeDownIcon;
    [SerializeField] Sprite SoloBatteryDownIcon;
    [SerializeField] Sprite GroupBatteryDownIcon;
    [SerializeField] Sprite SoloSoulDownIcon;
    [SerializeField] Sprite GroupSoulDownIcon;
    [SerializeField] Sprite SoloDebuffIcon;
    [SerializeField] Sprite GroupDebuffIcon;

    Color positiveColor = new Color(0f/255f, 223f/255f, 255f/255f, 1f); 
    Color negativeColor = new Color(255f/255f, 89f/255f, 0f/255f, 1f); 

    public void SetAttack(Attack attack)
    {
        SetIcon(attack.AttackType);
        SetAttackVal(attack);
    }

    private void SetIcon(AttackType attackType)
    {
        switch (attackType)
        {
            case AttackType.SoloLifeUp:
                attackIcon.sprite = SoloLifeUpIcon;
                break;
            case AttackType.GroupLifeUp:
                attackIcon.sprite = GroupLifeUpIcon;
                break;
            case AttackType.SoloBatteryUp:
                attackIcon.sprite = SoloBatteryUpIcon;
                break;
            case AttackType.GroupBatteryUp:
                attackIcon.sprite = GroupBatteryUpIcon;
                break;
            case AttackType.SoloSoulUp:
                attackIcon.sprite = SoloSoulUpIcon;
                break;
            case AttackType.GroupSoulUp:
                attackIcon.sprite = GroupSoulUpIcon;
                break;
            case AttackType.SoloGuard:
                attackIcon.sprite = SoloGuardIcon;
                break;
            case AttackType.GroupGuard:
                attackIcon.sprite = GroupGuardIcon;
                break;
            case AttackType.SoloSecurity:
                attackIcon.sprite = SoloSecurityIcon;
                break;
            case AttackType.GroupSecurity:
                attackIcon.sprite = GroupSecurityIcon;
                break;
            case AttackType.SoloBuff:
                attackIcon.sprite = SoloBuffIcon;
                break;
            case AttackType.GroupBuff:
                attackIcon.sprite = GroupBuffIcon;
                break;
            case AttackType.SoloLifeDown:
                attackIcon.sprite = SoloLifeDownIcon;
                break;
            case AttackType.GroupLifeDown:
                attackIcon.sprite = GroupLifeDownIcon;
                break;
            case AttackType.SoloBatteryDown:
                attackIcon.sprite = SoloBatteryDownIcon;
                break;
            case AttackType.GroupBatteryDown:
                attackIcon.sprite = GroupBatteryDownIcon;
                break;
            case AttackType.SoloSoulDown:
                attackIcon.sprite = SoloSoulDownIcon;
                break;
            case AttackType.GroupSoulDown:
                attackIcon.sprite = GroupSoulDownIcon;
                break;
            case AttackType.SoloDebuff:
                attackIcon.sprite = SoloDebuffIcon;
                break;
            case AttackType.GroupDebuff:
                attackIcon.sprite = GroupDebuffIcon;
                break;
            default:
                Debug.LogError("Unknown AttackType");
                break;
        }
    }

    private void SetAttackVal(Attack attack)
    {
        // 小数点以下は切り捨て
        valText.text = Mathf.Floor(attack.Val).ToString();
        // 小数点第1位まで表示
        timesText.text = "× " + attack.Times.ToString("F1");
        if (attack.AttackType.AssigneeSelf())
        {
            valText.color = positiveColor;
            timesText.color = positiveColor;
        }
        else
        {
            valText.color = negativeColor;
            timesText.color = negativeColor;
        }
    }
}