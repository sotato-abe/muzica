using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MapLibraryWindows : SelectWindow
{
    [SerializeField] WorldBigMapWindow worldBigMapWindow;
    [SerializeField] private GameObject contentArea;
    [SerializeField] private SelectElement selectElementObject;
    private List<FieldBase> fieldBaseList;

    public override void Start()
    {
        SetListElement();
        ChangeActiveWindow(false);
    }

    public override void WindowOpen()
    {
        base.WindowOpen();
        worldBigMapWindow.WindowOpen();
    }

    public override void WindowClose()
    {
        base.WindowClose();
        worldBigMapWindow.WindowClose();
    }

    public override void EnterTargetWindow()
    {
        // 選択ウィンドウの末端なのでターゲット移動を止める
    }


    private void SetListElement()
    {
        // マップライブラリのリスト要素を設定する処理をここに追加
        foreach (Transform child in contentArea.transform)
        {
            Destroy(child.gameObject);
        }
        selectElements.Clear();
        SetFieldElements();
    }

    private void SetFieldElements()
    {
        // アイテムの状態（取得状態など）を表示するため、状態記録も取得する。
        fieldBaseList = FieldDatabase.Instance.GetAllFieldBases();
        for (int i = 0; i < fieldBaseList.Count; i++)
        {
            FieldBase fieldBase = fieldBaseList[i];
            SelectElement newElement = Instantiate(selectElementObject, contentArea.transform);
            newElement.SetElementText(fieldBase.FieldName);
            newElement.SetIndex(i);
            newElement.SetActiveCursol(false);
            selectElements.Add(newElement);
        }

        int initialIndex = GetCurrentIndex();
        selectElements[initialIndex].SetActiveCursol(true);
    }
}
