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


    bool currentStay = false;

    public virtual void SetCharacter(Character character, bool setEnergy = false)
    {
        character.Init();
        characterImage.sprite = character.Base.SquareSprite;
        if (setEnergy)
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

