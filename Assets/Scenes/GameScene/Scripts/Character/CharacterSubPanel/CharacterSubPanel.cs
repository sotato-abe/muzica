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
    [SerializeField] EnergyGauge energyGauge;
    [SerializeField] Image turnBar;
    Character character;
    bool currentStay = false;
    float fillAmount = 0f;

    public delegate void ActiveTurnDelegate(Character? character);
    public event ActiveTurnDelegate OnActiveTurn;

    private void Awake()
    {
        turnBar.gameObject.SetActive(false);
    }

    public virtual void SetCharacter(Character character, bool isEnergy = false)
    {
        character.Init();
        this.character = character;
        characterImage.sprite = character.Base.SquareSprite;
        if (isEnergy)
        {
            energyGauge.gameObject.SetActive(true);
            energyGauge.SetLifeGauge(character.MaxLife, character.Life);
            energyGauge.SetBatteryGauge(character.MaxBattery, character.Battery);
            energyGauge.SetSoulGauge(100, character.Soul);
        }
        else
        {
            energyGauge.gameObject.SetActive(false);
        }
    }

    public override void SetActive(bool activeFlg, Action onComplete = null)
    {
        base.SetActive(activeFlg, onComplete);
        currentStay = activeFlg;
    }

    public void BattleStart()
    {
        if (character == null) return;
        // ターンバーを開始
        currentStay = true;
        turnBar.gameObject.SetActive(true);
        fillAmount = 0f;
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
        // character.Base.Speedに応じてターンバーの速度を変更
        // fullになったらOnActiveTurnを呼び出す
        float speed = character.Base.Speed / 5f; // 速度を調整
        while (fillAmount < 1f)
        {
            fillAmount += Time.deltaTime * speed;
            turnBar.fillAmount = fillAmount;
            yield return null;
        }
        OnActiveTurn?.Invoke(character);
        fillAmount = 0f;
    }

    // ターンバーを一時停止
    public void StopTurnBar()
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
        StartCoroutine(StartTurnBar());
    }

    public IEnumerator SetTalkMessage(TalkMessage talkMessage)
    {
        if (!currentStay)
        {
            base.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }

        yield return blowingPanel.AddMessageList(talkMessage);

        // ★ 最新のstayリクエストを見てから判断する
        if (!currentStay)
        {
            yield return new WaitForSeconds(0.3f);
            base.SetActive(false);
        }
    }
}

