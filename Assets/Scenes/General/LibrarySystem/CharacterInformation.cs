using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CharacterInformation : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI homeTowwn;
    [SerializeField] TextMeshProUGUI birthDate;
    [SerializeField] TextMeshProUGUI description;

    public void SetCharacter(Character character)
    {
        title.text = character.Base.Name;
        // birthDate.text = character.Base.BirthDate;

        if (character.Base.Birthplace != null)
            homeTowwn.text = character.Base.Birthplace.FieldName;
        else
            homeTowwn.text = "???";
        if (character.Base.Description != null)
            description.text = character.Base.Description;
        else
            description.text = "???";
    }
}
