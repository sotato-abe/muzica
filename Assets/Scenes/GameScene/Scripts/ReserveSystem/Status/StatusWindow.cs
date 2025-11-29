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
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI expText;
    [SerializeField] ExpPercentageBar bar;
    [SerializeField] TextMeshProUGUI expCountText;
    [SerializeField] StatusCounter lifeCounter;
    [SerializeField] StatusCounter powerCounter;
    [SerializeField] StatusCounter techniqueCounter;
    [SerializeField] StatusCounter defenseCounter;
    [SerializeField] StatusCounter speedCounter;
    [SerializeField] StatusCounter luckCounter;
    [SerializeField] SkillPointParts skillPoint;
    private int currentLevel = 1; // 現在のレベル
    private int currentExp = 0; // 現在の経験値

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

    public void SetLevel()
    {
        int level = this.playerCharacter.Level;
        levelText.text = level.ToString();

        int exp = this.playerCharacter.Exp;
        if (currentExp != exp)
        {
            expText.text = exp.ToString() + " / 100";
        }
        if (currentLevel == level && currentExp == exp) return; // 変更がない場合は処理しない
        UpdateExpBar(level, exp);
    }

    private void UpdateExpBar(int level, int exp)
    {
        // 以前からのレベル差と新しい経験値を受け渡す
        int levelDifference = level - currentLevel;
        bar.SetExpBar(levelDifference, exp);
        currentLevel = level; // 現在のレベルを更新
        currentExp = exp; // 現在の経験値を更新
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
        PlayerController.Instance.SetUpPlayerEnergy();
        skillPoint.SetPoint(playerCharacter.SkillPoint);
    }
}
