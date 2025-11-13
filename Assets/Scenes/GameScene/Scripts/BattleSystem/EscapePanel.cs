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

    [SerializeField] CostIconPrefab costIconPrefab;
    [SerializeField] GameObject costList;
    [SerializeField] private Image activeButtonImage;

    [SerializeField] Image runningBar1;
    [SerializeField] Image runningBar2;
    [SerializeField] Image runningBar3;
    [SerializeField] Image runningBar4;

    List<Character> enemyList = new List<Character>();
    Color activeColor = new Color(168f / 255f, 255f / 255f, 0f / 255f, 200f / 255f);
    Color stopColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 10f / 255f);

    private Color defaultButtonColor = new Color(130f / 255f, 130f / 255f, 130f / 255f, 255f / 255f);
    private Color activeButtonColor = new Color(240f / 255f, 88f / 255f, 0f / 255f, 255f / 255f);

    int lifeCost = 0;
    int batteryCost = 0;
    int soulCost = 0;

    private List<EnergyCost> energyCostList = new List<EnergyCost>();

    int probability = 0;
    private bool isEscaping = false;


    private void OnEnable()
    {
        ProbabilityCalculation();
        CountEnergyCost();
        if (canExecuteActionFlg)
            activeButtonImage.color = activeButtonColor;
        else
            activeButtonImage.color = defaultButtonColor;
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
        if (!canExecuteActionFlg && !isEscaping) return;
        if (!TryUseEnergy()) return;

        StartCoroutine(Escape());
    }

    private bool TryUseEnergy()
    {
        List<EnergyCost> energyCosts = new List<EnergyCost>();
        if (lifeCost > 0)
            energyCosts.Add(new EnergyCost(EnergyType.Life, lifeCost));
        if (batteryCost > 0)
            energyCosts.Add(new EnergyCost(EnergyType.Battery, batteryCost));
        if (soulCost > 0)
            energyCosts.Add(new EnergyCost(EnergyType.Soul, soulCost));
        if( energyCosts.Count == 0)
            return true;
        bool isUsed = PlayerController.Instance.UseEnergyCost(energyCosts);
        return isUsed;
    }

    public void SetEnemyList(List<Character> enemyList)
    {
        this.enemyList = enemyList;
        ProbabilityCalculation();
        CountEnergyCost();
    }

    public override void ChangeExecuteActionFlg(bool canExecute)
    {
        base.ChangeExecuteActionFlg(canExecute);
        if (canExecute)
            activeButtonImage.color = activeButtonColor;
        else
            activeButtonImage.color = defaultButtonColor;
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
        soulCost = player.Soul / 5;

        energyCostList.Clear();
        if (lifeCost > 0)
            energyCostList.Add(new EnergyCost(EnergyType.Life, lifeCost));
        if (batteryCost > 0)
            energyCostList.Add(new EnergyCost(EnergyType.Battery, batteryCost));
        if (soulCost > 0)
            energyCostList.Add(new EnergyCost(EnergyType.Soul, soulCost));
        SetCost(energyCostList);
    }

    private void SetCost(List<EnergyCost> energyCostList)
    {
        foreach (Transform child in costList.transform)
        {
            Destroy(child.gameObject);
        }

        // Costを表示する処理
        foreach (var cost in energyCostList)
        {
            CostIconPrefab newCost = Instantiate(costIconPrefab, costList.transform);
            newCost.SetCostIcon(cost);
        }
    }

    private IEnumerator Escape()
    {
        EnergyCost lifeEnergyCost = new EnergyCost(EnergyType.Life, lifeCost);
        EnergyCost batteryEnergyCost = new EnergyCost(EnergyType.Battery, batteryCost);
        EnergyCost soulEnergyCost = new EnergyCost(EnergyType.Soul, soulCost);
        List<EnergyCost> energyCosts = new List<EnergyCost> { lifeEnergyCost, batteryEnergyCost, soulEnergyCost };
        PlayerController.Instance.UseEnergyCost(energyCosts);

        yield return StartCoroutine(RunningCoroutine());
        int rand = Random.Range(0, 100);
        if (rand < probability)
        {
            // 逃げる成功
            yield return new WaitForSeconds(1f);
            TalkMessage runAwayMessage = new TalkMessage(MessageType.Escape, MessagePanelType.Default, "にげろ！");
            PlayerController.Instance.SetPlayerMessage(runAwayMessage);
            OnEscape?.Invoke();
        }
        else
        {
            // 逃げる失敗
            UnityEngine.Debug.Log($"{probability}/{rand}");
            PlayerController.Instance.SetPlayerMessageByType(MessageType.Miss);
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
