using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatePanelController : MonoBehaviour
{
    [SerializeField] private Image statePanel;
    [SerializeField] private Image stopPanel;
    [SerializeField] private Image playPanel;
    [SerializeField] private Image fastPanel;
    [SerializeField] private TextMeshProUGUI stateText;
    Color32 stopColor = new Color32(214, 51, 36, 255);
    Color32 liveColor = new Color32(3, 169, 244, 255);
    Color32 fastColor = new Color32(129, 67, 214, 255);


    // カラーを変更するメソッド
    public void ChangeState(TimeState state)
    {
        if (state == TimeState.Stop)
        {
            statePanel.color = stopColor; // 修正箇所
            stopPanel.GetComponent<Image>().color = stopColor;
            playPanel.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
            fastPanel.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
            stateText.text = "STOP";
        }
        else if (state == TimeState.Live)
        {
            statePanel.color = liveColor; // 修正箇所
            stopPanel.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
            playPanel.GetComponent<Image>().color = liveColor;
            fastPanel.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
            stateText.text = "LIVE";
        }
        else
        {
            statePanel.color = fastColor; // 修正箇所
            stopPanel.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
            playPanel.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
            fastPanel.GetComponent<Image>().color = fastColor;
            stateText.text = "FAST";
        }
    }
}
