using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Information
{
    [SerializeField] protected InformationBase _base;
    public virtual InformationBase Base => _base;
    public bool isCompleted = true;

    public Information(InformationBase baseData)
    {
        _base = baseData;
        this.isCompleted = false;
    }

    public virtual void Init()
    {
        // 初期化処理をここに追加
    }

    public virtual Information Clone()
    {
        return new Information(_base);
    }

    public InformationType GetInformationType()
    {
        return _base.InformationType;
    }

    public bool IsCompleted()
    {
        return isCompleted;
    }
}
