using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSubPanel : SlidePanel
{
    [SerializeField] Image characterImage;
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] public BlowingPanel blowingPanel;
    [SerializeField] public EnergyGauge energyGauge;
    [SerializeField] public Image turnBar;

    Character character;
    public Character Character => character;
    public bool fixedDisplayFlg = false; // Panelの固定表示フラグ
    bool isActive = false; // Panelがアクティブかどうか
    float turnBarFillAmount = 0f;
    Color runningColor = new Color(255f / 255f, 0f / 255f, 74f / 255f, 1f);
    Color activeColor = new Color(189f / 255f, 255f / 255f, 0f / 255f, 1f);

    public delegate void ActiveTurnDelegate(Character? character);
    public event ActiveTurnDelegate OnActiveTurn;

    public delegate void LifeOutDelegate(CharacterSubPanel characterSubPanel);
    public event LifeOutDelegate OnLifeOutAction;

    private void Awake()
    {
        turnBar.gameObject.SetActive(false);
    }

    public virtual void SetCharacter(Character character)
    {
        character.Init();
        this.character = character;
        characterImage.sprite = character.Base.SquareSprite;
        characterNameText.text = character.Base.Name;
        energyGauge.gameObject.SetActive(false);
        turnBar.gameObject.SetActive(false);
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
        base.SetActive(activeFlg, onComplete);
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
        characterNameText.text = character.Base.Name;
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
        character.TakeAttack(totalCount);
        yield return StartCoroutine(UpdateEnergyGauges());

        if (character.Life <= 0)
        {
            Debug.LogWarning("エネルギーが不足しています。");
            turnBar.gameObject.SetActive(false);
            StopAllCoroutines();
            OnLifeOutAction?.Invoke(this);
        }
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator UpdateEnergyGauges()
    {
        // 各ゲージの更新コルーチンを並行実行
        Coroutine lifeCoroutine = StartCoroutine(energyGauge.SetLifeValueCoroutine(character.Life));
        Coroutine batteryCoroutine = StartCoroutine(energyGauge.SetBatteryValueCoroutine(character.Battery));
        Coroutine soulCoroutine = StartCoroutine(energyGauge.SetSoulValueCoroutine(character.Soul));

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
}

