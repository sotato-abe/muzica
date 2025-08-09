using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StatusWindow : MonoBehaviour
{
    PlayerCharacter playerCharacter;
    [SerializeField] TextMeshProUGUI lvText;
    [SerializeField] Image lvBar;
    [SerializeField] TextMeshProUGUI expCountText;
    [SerializeField] StatusCounter lifeCounter;
    [SerializeField] StatusCounter powerCounter;
    [SerializeField] StatusCounter techniqueCounter;
    [SerializeField] StatusCounter defenseCounter;
    [SerializeField] StatusCounter speedCounter;
    [SerializeField] StatusCounter luckCounter;
    [SerializeField] SkillPointParts skillPoint;

    private void Awake()
    {
        lifeCounter.OnStatusUp += RestateWindow;
        powerCounter.OnStatusUp += RestateWindow;
        techniqueCounter.OnStatusUp += RestateWindow;
        defenseCounter.OnStatusUp += RestateWindow;
        speedCounter.OnStatusUp += RestateWindow;
        luckCounter.OnStatusUp += RestateWindow;
    }

    private void OnEnable()
    {
        RestateWindow();
        SetLevel();
    }

    public void SetCharacter(PlayerCharacter playerCharacter)
    {
        this.playerCharacter = playerCharacter;
        SetStatusCounters();
        SetLevel();
    }

    private void SetLevel()
    {
        lvText.text = playerCharacter.Level.ToString();
        expCountText.text = playerCharacter.Exp.ToString() + " / 100"; // 仮の値として100を使用
        if (playerCharacter.Exp > 0)
        {
            lvBar.fillAmount = (float)playerCharacter.Exp / 100f; // 仮の値として100を使用
        }
        else
        {
            lvBar.fillAmount = 0f;
        }
        skillPoint.SetPoint(playerCharacter.SkillPoint);
    }

    private void SetStatusCounters()
    {
        lifeCounter.SetCounter(playerCharacter.MaxLife, playerCharacter.ColLife);
        powerCounter.SetCounter(playerCharacter.Power, playerCharacter.ColPower);
        techniqueCounter.SetCounter(playerCharacter.Technique, playerCharacter.ColTechnique);
        defenseCounter.SetCounter(playerCharacter.Defense, playerCharacter.ColDefense);
        speedCounter.SetCounter(playerCharacter.Speed, playerCharacter.ColSpeed);
        luckCounter.SetCounter(playerCharacter.Luck, playerCharacter.ColLuck);
        // 他のステータスも同様に設定
    }

    public void RestateWindow()
    {
        SetStatusCounters();
        skillPoint.SetPoint(playerCharacter.SkillPoint);
    }
}
