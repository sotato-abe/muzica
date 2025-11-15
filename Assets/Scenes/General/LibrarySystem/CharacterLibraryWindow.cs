using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterLibraryWindows : SelectWindow
{
    [SerializeField] private GameObject contentArea;
    [SerializeField] private SelectElement selectElementObject;
    [SerializeField] CharacterCard characterCard;
    [SerializeField] CharacterInformation characterInformation;
    [SerializeField] CharacterStatus characterStatus;

    private List<CharacterBase> characterBaseList;

    private void Start()
    {
        SetListElement();
        ChangeActiveWindow(false);
    }

    private void SetListElement()
    {
        foreach (Transform child in contentArea.transform)
        {
            Destroy(child.gameObject);
        }
        characterBaseList = CharacterDatabase.Instance.GetAllCharacterBases();
        for (int i = 0; i < characterBaseList.Count; i++)
        {
            CharacterBase characterBase = characterBaseList[i];
            SelectElement newElement = Instantiate(selectElementObject, contentArea.transform);
            newElement.SetElementText(characterBase.Name);
            newElement.SetIndex(i);
            newElement.SetActiveCursol(false);
            selectElements.Add(newElement);
        }

        int initialIndex = GetCurrentIndex();
        selectElements[initialIndex].SetActiveCursol(true);
        TargetElement(initialIndex);
    }

    public override void TargetElement(int index)
    {
        CharacterBase characterBase = characterBaseList[index];
        Character targetCharacter = new Character(characterBase);
        characterCard.Setup(targetCharacter);
        characterInformation.SetCharacter(targetCharacter);
        characterStatus.SetCharacter(targetCharacter);
    }
}
