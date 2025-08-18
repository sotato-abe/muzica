using UnityEngine;
using TMPro;
using Newtonsoft.Json;

public class SaveManagement : MonoBehaviour
{
    public Setting setting;
    public const string RELATIVE_PATH = "setting.json";

    public TextMeshProUGUI text;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Persistance.Save(RELATIVE_PATH, setting);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Load setting");
            setting = Persistance.Load<Setting>(RELATIVE_PATH);
        }

        text.text = JsonConvert.SerializeObject(setting, Formatting.Indented);
    }
}

[System.Serializable]
public class Setting
{
    public NumText num;
    public int resolutionIndex = 0;
    public bool isFullScreen = true;
}

public enum NumText
{
    One,
    Two,
    Three,
    Four,
    Five
}
