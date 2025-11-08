using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Diagnostics;

public class BattleCharacterSubPanel : CharacterSubPanel, IDropHandler, IPointerClickHandler
{
    // public UnityAction OnEnergyOut;
    [SerializeField] public EnergyGauge energyGauge;
    [SerializeField] public Image turnBar;
    [SerializeField] Image targetImage;

    public FieldCharacter FieldCharacter { get; private set; }
    bool inProgress = false; // Panelがアクティブかどうか
    public bool isTarget = false; // ターゲット状態かどうか
    float turnBarFillAmount = 0f;
    Color runningColor = new Color(255f / 255f, 0f / 255f, 200f / 255f, 1f);
    Color activeColor = new Color(196f / 255f, 255f / 255f, 0 / 255f, 1f);

    private Coroutine turnBarCoroutine; // ターンバーのコルーチン参照

    // public UnityAction OnLifeOut;

    public delegate void TargetCharacterDelegate(BattleCharacterSubPanel characterSubPanel);
    public event TargetCharacterDelegate OnTarget;

    public delegate void ActiveTurnDelegate(BattleCharacterSubPanel characterSubPanel);
    public event ActiveTurnDelegate OnActiveTurn;

    public delegate void LifeOutDelegate(BattleCharacterSubPanel characterSubPanel);
    public event LifeOutDelegate OnLifeOutAction;

    #region Character Setup
    public void SetFieldCharacter(FieldCharacter fieldCharacter)
    {
        FieldCharacter = fieldCharacter;
    }

    public override void SetCharacter(Character character)
    {
        base.SetCharacter(character);
        energyGauge.gameObject.SetActive(true);
        turnBar.gameObject.SetActive(true);
        SetEnergy();
        SetMessageByType(MessageType.Encount);
        BattleStart();
    }

    public void SetEnergy()
    {
        if (Character == null)
        {
            UnityEngine.Debug.LogWarning("Character is not set.");
            return;
        }
        energyGauge.SetLifeGauge(Character.MaxLife, Character.Life);
        energyGauge.SetBatteryGauge(Character.MaxBattery, Character.Battery);
        energyGauge.SetSoulGauge(100, Character.Soul);
    }
    #endregion

    public void OnPointerClick(PointerEventData eventData)
    {
        OnTarget?.Invoke(this);
    }

    public void SetTarget(bool targetFlg)
    {
        if (Character == null) return;
        isTarget = targetFlg;
        targetImage.gameObject.SetActive(isTarget);
    }

    public void OnDrop(PointerEventData eventData)
    {
        // ドロップアイテムをバックに追加
        ItemBlock droppedItemBlock = eventData.pointerDrag?.GetComponent<ItemBlock>();

        if (droppedItemBlock != null && droppedItemBlock.Item != null)
        {
            Item item = droppedItemBlock.Item;
            if (item is Consumable && droppedItemBlock.isOwned)
            {
                Consumable consumable = item as Consumable;
                UseConsumable(consumable);
                bool isUsable = consumable.UseConsumable();
                if (!isUsable)
                {
                    droppedItemBlock.RemoveItem();
                }
                return;
            }
        }
    }

    private void UseConsumable(Consumable consumable)
    {
        TotalAttackCount totalCount = new TotalAttackCount
        {
            TargetType = consumable.ConsumableBase.TargetType,
            EnergyAttackList = consumable.ConsumableBase.EnergyAttackList,
            EnchantList = consumable.ConsumableBase.EnchantList
        };
        Character.TakeAttack(totalCount);
        StartCoroutine(UpdateEnergyGauges());
    }

    public void BattleStart()
    {
        UnityEngine.Debug.Log("Battle Start initiated.");
        if (Character == null) return;
        UnityEngine.Debug.Log($"Character {Character.Base.Name} is starting battle.");
        // ターンバーを開始
        this.gameObject.SetActive(true);
        fixedDisplayFlg = true;
        turnBar.gameObject.SetActive(true);
        turnBarFillAmount = 0f;
        turnBarCoroutine = StartCoroutine(StartTurnBar());
    }

    public IEnumerator TakeAttackCoroutine(TotalAttackCount totalCount)
    {
        if (Character == null) yield return null;

        StartCoroutine(JumpMotion());
        Character.TakeAttack(totalCount);
        FieldCharacter.SetAnimation(AnimationType.Damage);
        yield return StartCoroutine(UpdateEnergyGauges());

        if (Character.Life <= 0)
        {
            LifeOutAction();
        }
        yield return new WaitForSeconds(0.5f);
    }

    private void LifeOutAction()
    {
        // ターンバーのコルーチンのみを停止（他のコルーチンは継続）
        if (turnBarCoroutine != null)
        {
            StopCoroutine(turnBarCoroutine);
            turnBarCoroutine = null;
        }
        StartCoroutine(LifeOut());
    }

    public virtual IEnumerator LifeOut()
    {
        UnityEngine.Debug.Log($"Character {Character.Base.Name} is executing LifeOut.");
        turnBar.gameObject.SetActive(false);
        TalkMessage talkMessage = Character.GetTalkMessageByType(MessageType.Lose);
        FieldCharacter.SetAnimation(AnimationType.Death);
        yield return StartCoroutine(SetTalkMessage(talkMessage));
        yield return new WaitForSeconds(1f);
        base.SetActive(false);
        Destroy(FieldCharacter.gameObject);
        OnLifeOutAction?.Invoke(this);
        yield return null;
    }

    public IEnumerator UpdateEnergyGauges()
    {
        // 各ゲージの更新コルーチンを並行実行
        Coroutine lifeCoroutine = StartCoroutine(energyGauge.SetLifeValueCoroutine(Character.Life, Character.MaxLife, Character.LifeGuard));
        Coroutine batteryCoroutine = StartCoroutine(energyGauge.SetBatteryValueCoroutine(Character.Battery, Character.MaxBattery, Character.BatteryGuard));
        Coroutine soulCoroutine = StartCoroutine(energyGauge.SetSoulValueCoroutine(Character.Soul, 100, 0));

        // すべてのコルーチンが完了するまで待機
        yield return lifeCoroutine;
        yield return batteryCoroutine;
        yield return soulCoroutine;
    }

    private IEnumerator StartTurnBar()
    {
        UnityEngine.Debug.Log("Starting turn bar...");
        if (Character == null)
        {
            UnityEngine.Debug.LogWarning("キャラクターが設定されていません。");
            yield break;
        }
        turnBar.color = runningColor;
        float speed = Character.ColSpeed / 20f; // 速度を調整
        while (turnBarFillAmount < 1f)
        {
            turnBarFillAmount += Time.deltaTime * speed;
            turnBar.fillAmount = turnBarFillAmount;
            yield return null;
        }
        turnBar.color = activeColor;
        inProgress = true;
        OnActiveTurn?.Invoke(this);
    }

    // ターンバーを一時停止
    public void PauseTurnBar()
    {
        if (turnBarCoroutine != null)
        {
            StopCoroutine(turnBarCoroutine);
            turnBarCoroutine = null;
        }
    }

    // ターンバーを再開
    public void ReStartTurnBar()
    {
        if (Character == null)
        {
            UnityEngine.Debug.LogWarning("キャラクターが設定されていません。");
            return;
        }
        if (inProgress)
        {
            turnBarFillAmount = 0f; // ターンバーが満タンの場合はリセット
            inProgress = false;
        }
        turnBarCoroutine = StartCoroutine(StartTurnBar());
    }
}

