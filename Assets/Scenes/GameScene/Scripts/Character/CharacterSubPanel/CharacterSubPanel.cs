using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSubPanel : SlidePanel
{
    [SerializeField] Image characterImage;
    [SerializeField] BlowingPanel blowingPanel;
    [SerializeField] public EnergyGauge energyGauge;
    [SerializeField] public Image turnBar;
    Character character;
    bool fixedDisplayFlg = false; // Panelの固定表示フラグ
    bool isActive = false; // Panelがアクティブかどうか
    float turnBarFillAmount = 0f;
    Color runningColor = new Color(255f / 255f, 0f / 255f, 74f / 255f, 1f);
    Color activeColor = new Color(189f / 255f, 255f / 255f, 0f / 255f, 1f);

    public delegate void ActiveTurnDelegate(Character? character);
    public event ActiveTurnDelegate OnActiveTurn;

    private void Awake()
    {
        turnBar.gameObject.SetActive(false);
    }

    public virtual void SetCharacter(Character character)
    {
        character.Init();
        this.character = character;
        characterImage.sprite = character.Base.SquareSprite;
        energyGauge.gameObject.SetActive(false);
        turnBar.gameObject.SetActive(false);
    }

    public override void SetActive(bool activeFlg, Action onComplete = null)
    {
        base.SetActive(activeFlg, onComplete);
        fixedDisplayFlg = activeFlg;
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

    // Battle用のキャラクターを設定
    public virtual void SetBattleCharacter(Character character)
    {
        character.Init();
        this.character = character;
        characterImage.sprite = character.Base.SquareSprite;
        energyGauge.gameObject.SetActive(true);
        turnBar.gameObject.SetActive(true);
        SetEnergy();
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
        fixedDisplayFlg = true;
        turnBar.gameObject.SetActive(true);
        turnBarFillAmount = 0f;
        StartCoroutine(StartTurnBar());
    }

    public void BattleEnd()
    {
        // ターンバーを停止
        turnBar.gameObject.SetActive(false);
    }

    private IEnumerator StartTurnBar()
    {
        if (character == null)
        {
            Debug.LogWarning("キャラクターが設定されていません。");
            yield break;
        }
        turnBar.color = runningColor;
        float speed = character.Base.Speed / 5f; // 速度を調整
        while (turnBarFillAmount < 1f)
        {
            turnBarFillAmount += Time.deltaTime * speed;
            turnBar.fillAmount = turnBarFillAmount;
            yield return null;
        }
        turnBar.color = activeColor;
        isActive = true;
        OnActiveTurn?.Invoke(character);
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
        if (isActive)
        {
            turnBarFillAmount = 0f; // ターンバーが満タンの場合はリセット
            isActive = false;
        }
        StartCoroutine(StartTurnBar());
    }
}

