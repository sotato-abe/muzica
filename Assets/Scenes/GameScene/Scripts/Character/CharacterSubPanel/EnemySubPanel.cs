using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemySubPanel : BattleCharacterSubPanel
{
    [SerializeField] SwitchImage numberImage;

    private int enemyIndex;

    public void SetEnemy(Character character, FieldCharacter fieldCharacter)
    {
        SetFieldCharacter(fieldCharacter);
        SetCharacter(character);
    }

    public void SetEnemyIndex(int index)
    {
        enemyIndex = index;
    }

    public void SetNumber(int number)
    {
        numberImage.Switch(number);
    }
}

