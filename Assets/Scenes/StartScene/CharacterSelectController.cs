using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacterSelectController : MonoBehaviour
{

    [SerializeField] List<CharacterBase> characterBaseList;
    [SerializeField] CharacterSelectButton characterButtonPrefab;
    [SerializeField] CharacterCard characterCard;

    public delegate void CharacterSelectDelegate(int index);
    public event CharacterSelectDelegate OnCharacterSelect;

    private List<CharacterSelectButton> characterSelectButtons = new List<CharacterSelectButton>();
    private Character selectedCharacter;

    private void Start()
    {
        CreateCharacterSelectButtons();
        CharacterSelect(0);
    }


    private void CreateCharacterSelectButtons()
    {
        characterSelectButtons.Clear();
        ClearSelectButtons();
        for (int i = 0; i < characterBaseList.Count; i++)
        {
            CharacterSelectButton button = Instantiate(characterButtonPrefab, this.gameObject.transform);
            button.SetCharacter(i, characterBaseList[i].SquareSprite);
            button.OnSelectCharacter += CharacterSelect;
            characterSelectButtons.Add(button);
        }
    }

    private void ClearSelectButtons()
    {
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void CharacterSelect(int index)
    {
        OnCharacterSelect?.Invoke(index);
        selectedCharacter = new Character(characterBaseList[index]);
        characterCard.Setup(selectedCharacter);
        for (int i = 0; i < characterSelectButtons.Count; i++)
        {
            characterSelectButtons[i].SelectButton(i == index);
        }
    }
}
