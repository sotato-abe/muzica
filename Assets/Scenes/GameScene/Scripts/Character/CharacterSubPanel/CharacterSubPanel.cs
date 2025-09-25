using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CharacterSubPanel : SlidePanel, IDropHandler, IPointerClickHandler
{
    [SerializeField] Image characterImage;
    [SerializeField] Image frame;
    [SerializeField] Image nameFrame;
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] public BlowingPanel blowingPanel;
    [SerializeField] public EnergyGauge energyGauge;
    [SerializeField] public Image turnBar;
    [SerializeField] Image targetImage;
    [SerializeField] SwitchImage numberImage;

    Character character;
    public Character Character => character;
    public bool fixedDisplayFlg = false; // Panelの固定表示フラグ
    bool inProgress = false; // Panelがアクティブかどうか
    public bool isTarget = false; // ターゲット状態かどうか
    float turnBarFillAmount = 0f;
    Color runningColor = new Color(255f / 255f, 0f / 255f, 200f / 255f, 1f);
    Color activeColor = new Color(196f / 255f, 255f / 255f, 0 / 255f, 1f);

    public UnityAction OnLifeOut;

    public delegate void TargetCharacterDelegate(CharacterSubPanel characterSubPanel);
    public event TargetCharacterDelegate OnTarget;

    public delegate void ActiveTurnDelegate(CharacterSubPanel characterSubPanel);
    public event ActiveTurnDelegate OnActiveTurn;

    public delegate void LifeOutDelegate(CharacterSubPanel characterSubPanel);
    public event LifeOutDelegate OnLifeOutAction;

    private void Awake()
    {
        turnBar.gameObject.SetActive(false);
        SetTarget(false);
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
                bool isUsable = UseConsumable(item as Consumable);
                if (!isUsable)
                {
                    droppedItemBlock.RemoveItem();
                }
                return;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnTarget?.Invoke(this);
    }

    public virtual void SetCharacter(Character character)
    {
        this.character = character;
        characterImage.sprite = character.Base.SquareSprite;
        characterNameText.text = character.Base.Name;
        energyGauge.gameObject.SetActive(false);
        turnBar.gameObject.SetActive(false);
        SetRarityColor();
    }

    public void SetNumber(int number)
    {
        numberImage.Switch(number);
    }

    public void SetOwner(Character character)
    {
        this.character = character;
        characterImage.sprite = character.Base.SquareSprite;
        characterNameText.text = character.Base.Name;
        energyGauge.gameObject.SetActive(false);
        turnBar.gameObject.SetActive(false);
        SetActive(true);
        SetMessageByType(MessageType.Entrance);
        SetRarityColor();
    }

    public virtual void SetEnemy(Character character)
    {
        this.character = character;
        characterImage.sprite = character.Base.SquareSprite;
        characterNameText.text = character.Base.Name;
        energyGauge.gameObject.SetActive(true);
        turnBar.gameObject.SetActive(true);
        SetEnergy();
        SetActive(true);
        SetMessageByType(MessageType.Encount);
        BattleStart();
        SetRarityColor();
    }

    private void SetRarityColor()
    {
        if (character == null) return;
        Color rarityColor = character.Base.Rarity.GetRarityColor();
        frame.color = rarityColor;
        nameFrame.color = rarityColor;
    }

    public void SetTarget(bool targetFlg)
    {
        if(character == null) return;
        isTarget = targetFlg;
        targetImage.gameObject.SetActive(isTarget);
    }

    private bool UseConsumable(Consumable consumable)
    {
        TotalAttackCount totalCount = new TotalAttackCount
        {
            TargetType = consumable.ConsumableBase.TargetType,
            EnergyAttackList = consumable.ConsumableBase.EnergyAttackList,
            EnchantList = consumable.ConsumableBase.EnchantList
        };
        character.TakeAttack(totalCount);
        bool isUsable = consumable.UseConsumable();
        StartCoroutine(UpdateEnergyGauges());
        return isUsable;
    }

    public void SetMessageByType(MessageType messageType)
    {
        TalkMessage talkMessage = character.GetTalkMessageByType(messageType);
        if (talkMessage != null)
        {
            StartCoroutine(SetTalkMessage(talkMessage));
        }
    }

    public IEnumerator SetTalkMessage(TalkMessage talkMessage)
    {
        if (!fixedDisplayFlg)
        {
            base.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }

        yield return blowingPanel.AddMessageList(talkMessage);

        if (!fixedDisplayFlg)
        {
            yield return new WaitForSeconds(0.3f);
            base.SetActive(false);
        }
    }

    public void SetEnergy()
    {
        if (character == null)
        {
            Debug.LogWarning("Character is not set.");
            return;
        }
        energyGauge.SetLifeGauge(character.MaxLife, character.Life);
        energyGauge.SetBatteryGauge(character.MaxBattery, character.Battery);
        energyGauge.SetSoulGauge(100, character.Soul);
    }

    public void BattleStart()
    {
        if (character == null) return;
        // ターンバーを開始
        this.gameObject.SetActive(true);
        fixedDisplayFlg = true;
        turnBar.gameObject.SetActive(true);
        turnBarFillAmount = 0f;
        StartCoroutine(StartTurnBar());
    }

    public IEnumerator TakeAttackCoroutine(TotalAttackCount totalCount)
    {
        if (character == null) yield return null;

        StartCoroutine(JumpMotion());
        TalkMessage talkMessage = character.GetTalkMessageByType(MessageType.Damage);
        StartCoroutine(SetTalkMessage(talkMessage));
        character.TakeAttack(totalCount);
        yield return StartCoroutine(UpdateEnergyGauges());

        if (character.Life <= 0)
        {
            Debug.LogWarning("戦闘不能");
            talkMessage = character.GetTalkMessageByType(MessageType.Lose);
            yield return StartCoroutine(SetTalkMessage(talkMessage));
            turnBar.gameObject.SetActive(false);
            StopAllCoroutines();
            OnLifeOut?.Invoke();
            OnLifeOutAction?.Invoke(this);
        }
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator UpdateEnergyGauges()
    {
        // 各ゲージの更新コルーチンを並行実行
        Coroutine lifeCoroutine = StartCoroutine(energyGauge.SetLifeValueCoroutine(character.Life, character.MaxLife, character.LifeGuard));
        Coroutine batteryCoroutine = StartCoroutine(energyGauge.SetBatteryValueCoroutine(character.Battery, character.MaxBattery, character.BatteryGuard));
        Coroutine soulCoroutine = StartCoroutine(energyGauge.SetSoulValueCoroutine(character.Soul, 100, 0));

        // すべてのコルーチンが完了するまで待機
        yield return lifeCoroutine;
        yield return batteryCoroutine;
        yield return soulCoroutine;
    }

    private IEnumerator StartTurnBar()
    {
        if (character == null)
        {
            Debug.LogWarning("キャラクターが設定されていません。");
            yield break;
        }
        turnBar.color = runningColor;
        float speed = character.ColSpeed / 20f; // 速度を調整
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
        StopAllCoroutines();
    }

    // ターンバーを再開
    public void ReStartTurnBar()
    {
        if (character == null)
        {
            Debug.LogWarning("キャラクターが設定されていません。");
            return;
        }
        if (inProgress)
        {
            turnBarFillAmount = 0f; // ターンバーが満タンの場合はリセット
            inProgress = false;
        }
        StartCoroutine(StartTurnBar());
    }

    private IEnumerator JumpMotion()
    {
        float groundY = transform.position.y;
        float bounceHeight = 40f;
        float damping = 0.4f;
        float gravity = 5000f;

        while (bounceHeight >= 0.1f)
        {
            float verticalVelocity = Mathf.Sqrt(2 * gravity * bounceHeight);
            bool isFalling = false;

            // 上昇と下降のループ
            while (transform.position.y >= groundY || !isFalling)
            {
                verticalVelocity -= gravity * Time.deltaTime;
                transform.position += Vector3.up * verticalVelocity * Time.deltaTime;

                if (transform.position.y <= groundY)
                {
                    isFalling = true;
                    break;
                }

                yield return null;
            }

            bounceHeight *= damping; // バウンドを減衰させる
        }

        transform.position = new Vector3(transform.position.x, groundY, transform.position.z); // 最後に位置を調整
    }

    public override void SetActive(bool activeFlg, Action onComplete = null)
    {
        fixedDisplayFlg = activeFlg;
        if (!activeFlg)
        {
            StopAllCoroutines();
            turnBar.gameObject.SetActive(false);
            blowingPanel.gameObject.SetActive(false);
        }
        this.gameObject.SetActive(true);
        base.SetActive(activeFlg, onComplete);
    }
}

