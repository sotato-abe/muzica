using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ability
{
    [SerializeField] AbilityBase _base;

    public AbilityBase Base { get => _base; }

    public Ability(AbilityBase abilityBase)
    {
        _base = abilityBase;
    }
}
