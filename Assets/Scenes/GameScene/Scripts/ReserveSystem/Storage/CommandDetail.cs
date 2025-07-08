using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CommandDetail : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI commandNameText;
    [SerializeField] TextMeshProUGUI commandDescriptionText;
    [SerializeField] Image commandImage;
    [SerializeField] GameObject costList;
    [SerializeField] EnegyCostIcon enegyCostIconPrefab;

    private void Awake()
    {
        ResetSlot();
    }

    public void SetCommandDetail(Command command)
    {
        commandNameText.text = command.Base.Name;
        commandDescriptionText.text = command.Base.Description;
        commandImage.sprite = command.Base.Sprite;
        commandImage.color = new Color(1, 1, 1, 1);
        this.gameObject.SetActive(true);
        SetCosts(command.Base.EnegyCostList);
    }

    private void SetCosts(List<EnegyCost> costs)
    {
        foreach (Transform child in costList.transform)
        {
            Destroy(child.gameObject);
        }

        // EnegyCostを表示する処理
        foreach (var cost in costs)
        {
            EnegyCostIcon newIcon = Instantiate(enegyCostIconPrefab, costList.transform);
            newIcon.SetCost(cost);
        }
    }

    public void ResetSlot()
    {
        commandImage.color = new Color(1, 1, 1, 0);

        foreach (Transform child in costList.transform)
        {
            Destroy(child.gameObject);
        }
        this.gameObject.SetActive(false);
    }
}
