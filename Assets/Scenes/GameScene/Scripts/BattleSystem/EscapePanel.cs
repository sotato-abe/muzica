using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class EscapePanel : BattleActionPanel
{
    public UnityAction OnEscape;
    public UnityAction OnActionEnd;

    [SerializeField] TextMeshProUGUI probabilityText;
    [SerializeField] TextMeshProUGUI lifeCostText;
    [SerializeField] TextMeshProUGUI batteryCostText;
    [SerializeField] TextMeshProUGUI soulCostText;
    [SerializeField] Image runningBar1;
    [SerializeField] Image runningBar2;
    [SerializeField] Image runningBar3;
    [SerializeField] Image runningBar4;

    List<Character> enemyList = new List<Character>();
    Color activeColor = new Color(168f / 255f, 255f / 255f, 0f / 255f, 200f / 255f);
    Color stopColor = new Color(0f / 255f, 0f / 255f, 0f / 255f, 200f / 255f);

    int lifeCost = 0;
    int batteryCost = 0;
    int soulCost = 0;
    int probability = 0;
    private bool isEscaping = false;


    private void OnEnable()
    {
        ProbabilityCalculation();
        CountEnergyCost();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteAction();
        }
    }

    public void ExecuteAction()
    {
        if (!canExecuteActionFlg && !isEscaping)
        {
            Debug.LogWarning("アクションが実行できません。");
            return;
        }

        StartCoroutine(Escape());
    }

    public void SetEnemyList(List<Character> enemyList)
    {
        this.enemyList = enemyList;
        ProbabilityCalculation();
        CountEnergyCost();
    }

    private void ProbabilityCalculation()
    {
        PlayerCharacter player = PlayerController.Instance.PlayerCharacter;
        int playerSPD = player.ColSpeed;
        int enemySPD = 0;
        foreach (var enemy in enemyList)
        {
            enemySPD += enemy.ColSpeed;
        }
        probability = (playerSPD * 100) / (playerSPD + enemySPD);
        probabilityText.SetText(probability.ToString());
    }

    private void CountEnergyCost()
    {
        PlayerCharacter player = PlayerController.Instance.PlayerCharacter;
        lifeCost = Mathf.Max(0, player.Life / 10);
        batteryCost = Mathf.Max(0, player.Battery / 10);
        soulCost = player.Soul / 2;

        lifeCostText.SetText(lifeCost.ToString());
        batteryCostText.SetText(batteryCost.ToString());
        soulCostText.SetText(soulCost.ToString());
    }

    private IEnumerator Escape()
    {
        EnergyCost lifeEnergyCost = new EnergyCost(EnergyType.Life, lifeCost);
        EnergyCost batteryEnergyCost = new EnergyCost(EnergyType.Battery, batteryCost);
        EnergyCost soulEnergyCost = new EnergyCost(EnergyType.Soul, soulCost);
        List<EnergyCost> energyCosts = new List<EnergyCost> { lifeEnergyCost, batteryEnergyCost, soulEnergyCost };
        PlayerController.Instance.UseEnergyCost(energyCosts);

        yield return StartCoroutine(RunningCoroutine());

        if (Random.Range(0, 100) < probability)
        {
            // 逃げる成功
            yield return new WaitForSeconds(1f);
            PlayerController.Instance.SetPlayerBlowing(new TalkMessage(MessageType.Escape, MessagePanelType.Default, "にげろ！"));
            OnEscape?.Invoke();
        }
        else
        {
            // 逃げる失敗
            PlayerController.Instance.SetPlayerBlowing(new TalkMessage(MessageType.Miss, MessagePanelType.Default, "くそっ！"));
        }
        RunningOff();
        CountEnergyCost();
        ProbabilityCalculation();
        isEscaping = false;
        OnActionEnd?.Invoke();
    }

    private IEnumerator RunningCoroutine()
    {
        // escapeBarは0から3までのランダムな値
        int escapeBar = Random.Range(1, 5);
        for (int i = 0; i < escapeBar; i++)
        {
            if (i == 0)
            {
                runningBar1.color = activeColor;
            }
            else if (i == 1)
            {
                runningBar2.color = activeColor;
            }
            else if (i == 2)
            {
                runningBar3.color = activeColor;
            }
            else if (i == 3)
            {
                runningBar4.color = activeColor;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void RunningOff()
    {
        // Corutinを停止
        StopAllCoroutines();
        runningBar1.color = stopColor;
        runningBar2.color = stopColor;
        runningBar3.color = stopColor;
        runningBar4.color = stopColor;
    }
}
