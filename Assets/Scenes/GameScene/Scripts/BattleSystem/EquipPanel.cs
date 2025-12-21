using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 装備を使用した攻撃アクションを管理するパネル
/// </summary>
public class EquipPanel : BattleActionPanel
{
    #region Events
    public UnityAction OnActionStart;
    public UnityAction OnActionEnd;
    #endregion

    #region Serialized Fields
    [Header("UI Components")]
    [SerializeField] private FieldPlayer fieldPlayer;
    [SerializeField] private SlotWindow slotWindow;
    [SerializeField] private EquipmentCard equipmentCard;
    [SerializeField] private Image activeButtonImage;

    [Header("Character Panels")]
    [SerializeField] private PlayerSubPanel playerSubPanel;
    [SerializeField] private EnemySubPanel enemySubPanel1;
    [SerializeField] private EnemySubPanel enemySubPanel2;
    [SerializeField] private EnemySubPanel enemySubPanel3;
    #endregion

    #region Private Fields
    private Equipment currentEquipment;
    private Color defaultButtonColor = new Color(130f / 255f, 130f / 255f, 130f / 255f, 255f / 255f);
    private Color activeButtonColor = new Color(240f / 255f, 88f / 255f, 0f / 255f, 255f / 255f);

    List<EnemySubPanel> enemySubPanels = new List<EnemySubPanel>(); // 敵のサブパネルリスト
    List<BattleCharacterSubPanel> targetSubPanels = new List<BattleCharacterSubPanel>();

    [Header("Equipment Settings")]
    public BodyPartType bodyPartType = BodyPartType.None;
    public int equipmentNum = 0;
    private TargetType targetType;
    private List<EnergyCost> energyCostList;
    private List<Enchant> enchantList;
    #endregion

    #region Unity Lifecycle
    protected override void Awake()
    {
        base.Awake();
        enemySubPanels.Add(enemySubPanel3);
        enemySubPanels.Add(enemySubPanel2);
        enemySubPanels.Add(enemySubPanel1);
    }

    private void OnEnable()
    {
        SetEquipment();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteEquipAction();
        }
    }
    #endregion

    #region Equipment Management
    public void SetEquipment()
    {
        UnityEngine.Debug.Log("EquipPanel: Setting equipment for body part " + bodyPartType);
        currentEquipment = PlayerController.Instance.GetEquipmentByBodyPart(bodyPartType);
        if (currentEquipment == null)
        {
            slotWindow.gameObject.SetActive(false);
            equipmentCard.gameObject.SetActive(false);
            currentEquipment = null;
            return;
        }
        slotWindow.gameObject.SetActive(true);
        equipmentCard.gameObject.SetActive(true);
        equipmentCard.SetCard(currentEquipment);
        ChangeEquipStatus();
    }

    private void ChangeEquipStatus()
    {
        if (!IsEquipmentValid())
        {
            activeButtonImage.color = defaultButtonColor;
            return;
        }
        bool canUse = PlayerController.Instance.CheckEnergyCost(currentEquipment.EquipmentBase.EnergyCostList);
        if (canUse && canExecuteActionFlg)
            activeButtonImage.color = activeButtonColor;
        else
            activeButtonImage.color = defaultButtonColor;
    }
    #endregion

    #region EquipAction Execution
    public override void ChangeExecuteActionFlg(bool canExecute)
    {
        base.ChangeExecuteActionFlg(canExecute);
        if (currentEquipment == null)
        {
            activeButtonImage.color = defaultButtonColor;
            return;
        }
        bool canUse = PlayerController.Instance.CheckEnergyCost(currentEquipment.EquipmentBase.EnergyCostList);
        if (canExecute && canUse)
            activeButtonImage.color = activeButtonColor;
        else
            activeButtonImage.color = defaultButtonColor;
    }

    public void ExecuteEquipAction()
    {
        if (!gameObject.activeInHierarchy) return;
        if (!canExecuteActionFlg) return;
        if (!IsEquipmentValid()) return;
        if (!TryUseEnergy()) return;

        SoundSystem.Instance.PlaySE(SeType.Play);
        OnActionStart?.Invoke();
        canExecuteActionFlg = false;
        StartCoroutine(StopSlot());
    }

    private bool TryUseEnergy()
    {
        bool isUsed = PlayerController.Instance.UseEnergyCost(currentEquipment.EquipmentBase.EnergyCostList);
        return isUsed;
    }

    private IEnumerator StopSlot()
    {
        yield return new WaitForSeconds(0.8f);
        yield return StartCoroutine(slotWindow.StopSlot());
        yield return new WaitForSeconds(0.5f);
        TotalAttack totalAttack = equipmentCard.GetTotalAttack();
        if (totalAttack.isPositiveAttack())
        {
            SoundSystem.Instance.PlaySE(SeType.Recovery);
        }
        PlayerController.Instance.TakeTotalAttack(totalAttack.GetPositiveTotalAttack(true));
        yield return StartCoroutine(ExecuteTargetAttack(totalAttack.GetPositiveTotalAttack(false)));
        yield return new WaitForSeconds(0.5f);
        RestartSlot();
        equipmentCard.ResetCurrentCard();
        OnActionEnd?.Invoke();
    }

    private IEnumerator ExecuteTargetAttack(TotalAttack totalAttack)
    {
        if (totalAttack.AttackList.Count == 0 && totalAttack.EnchantList.Count == 0)
            yield return null; // 攻撃がない場合は処理を終了
        List<Attack> attacks = totalAttack.AttackList;
        List<BattleCharacterSubPanel> targetSubPanels = new List<BattleCharacterSubPanel>();

        foreach (var attack in attacks)
        {
            TotalAttack singleAttackTotal = new TotalAttack
            {
                AttackList = new List<Attack> { attack },
                EnchantList = totalAttack.EnchantList
            };

            if (attack.AttackType.IsGroupAttack())
            {
                // 全体攻撃の場合、開いている全ての敵に攻撃を行う
                foreach (var targetSubPanel in enemySubPanels)
                {
                    if (!targetSubPanel.isOpen) continue;
                    SoundSystem.Instance.PlaySE(SeType.Damage);
                    fieldPlayer.SetAnimation(AnimationType.Attack);
                    yield return StartCoroutine(targetSubPanel.TakeAttackCoroutine(singleAttackTotal));
                }
            }
            else
            {
                // 単体攻撃の場合、最初に開いている敵に攻撃を行う
                foreach (var enemySubPanel in enemySubPanels)
                {
                    if (enemySubPanel.isOpen)
                    {
                        SoundSystem.Instance.PlaySE(SeType.Damage);
                        fieldPlayer.SetAnimation(AnimationType.Attack);
                        yield return StartCoroutine(enemySubPanel.TakeAttackCoroutine(singleAttackTotal));
                        break; // 最初に見つけた開いている敵に攻撃したらループを抜ける
                    }
                }
            }
        }
    }

    public void RestartSlot()
    {
        if (!IsEquipmentValid() || !gameObject.activeInHierarchy)
            return;
        equipmentCard.ResetCurrentCard();
        StartCoroutine(slotWindow.StartSlot());
    }
    #endregion

    private bool IsEquipmentValid()
    {
        if (currentEquipment == null)
        {
            UnityEngine.Debug.LogWarning("現在の装備が設定されていません。");
            return false;
        }
        return true;
    }

    public void LifeOutEnemy(BattleCharacterSubPanel enemySubPanel)
    {
        // gameObjectが非アクティブの場合は処理を中断
        if (!this.gameObject.activeInHierarchy) return;
        // targetSubPanelsからライフアウトした敵のサブパネルを削除
        targetSubPanels.Remove(enemySubPanel);
    }
}
