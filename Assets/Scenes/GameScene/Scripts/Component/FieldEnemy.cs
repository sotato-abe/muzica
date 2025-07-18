using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FieldEnemy : FieldCharacter
{
    [SerializeField] Sprite Number1;
    [SerializeField] Sprite Number2;
    [SerializeField] Sprite Number3;

    public void SetNumIcon(int? num)
    {
        switch (num)
        {
            case 1:
                IconImage.sprite = Number1;
                break;
            case 2:
                IconImage.sprite = Number2;
                break;
            case 3:
                IconImage.sprite = Number3;
                break;
            default:
                IconImage.sprite = null;
                break;
        }
    }
}
