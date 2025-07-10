using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Point
{
    [SerializeField] PointBase _base;

    public PointBase Base { get => _base; }
    List<ItemBase> ShopItemList  { get; set; }


    public virtual void Init()
    {
        if (_base == null)
        {
            Debug.LogError("Init() failed: _base is null");
            return;
        }

        ShopItemList = new List<ItemBase>(_base.ShopItems);
    }
}
