using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldBigMapWindow : Panel
{
    [SerializeField] WorldBigMapCameraManager worldMapCameraManager;
    // [SerializeField] AgeTimePanel ageTimePanel;

    private bool isCameraUpFlg = false;
    private bool isCameraBottomFlg = false;
    private bool isCameraRightFlg = false;
    private bool isCameraLeftFlg = false;

    void Update()
    {
        if (!isActive) return; // フラグがfalseの場合は処理をスキップ

        if (isCameraUpFlg || Input.GetKey(KeyCode.UpArrow))
        {
            worldMapCameraManager.UpTarget(); // 上に移動
        }
        if (isCameraBottomFlg || Input.GetKey(KeyCode.DownArrow))
        {
            worldMapCameraManager.DownTarget(); // 下に移動
        }
        if (isCameraRightFlg || Input.GetKey(KeyCode.RightArrow))
        {
            worldMapCameraManager.RightTarget(); // 右に移動
        }
        if (isCameraLeftFlg || Input.GetKey(KeyCode.LeftArrow))
        {
            worldMapCameraManager.LeftTarget(); // 左に移動
        }
        if (Input.inputString.Contains("@"))
        {
            OnCurrentPosition(); // 下に移動
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
        {
            WindowClose();
        }
    }

    public void SwitchActive()
    {
        isActive = !isActive; // フラグをトグル
        if (isActive)
        {
            // ageTimePanel.SetTimeSpeed(TimeState.Live); // 時間を止める
            WindowOpen();
        }
        else
        {
            // ageTimePanel.SetTimeSpeed(TimeState.Fast); // 時間を進める
            WindowClose();
        }
    }

    private void WindowClose()
    {
        isActive = false;
        // ageTimePanel.SetTimeSpeed(TimeState.Fast); // 時間を進める
        transform.gameObject.SetActive(false);
    }

    public void OnUpStart()
    {
        Debug.Log("Up button pressed");
        isCameraUpFlg = true;
    }

    public void OnUpEnd()
    {
        isCameraUpFlg = false;
    }

    public void OnDownStart()
    {
        isCameraBottomFlg = true;
    }
    public void OnDownEnd()
    {
        isCameraBottomFlg = false;
    }

    public void OnRightStart()
    {
        Debug.Log("Right button pressed");
        isCameraRightFlg = true;
    }

    public void OnRightEnd()
    {
        isCameraRightFlg = false;
    }

    public void OnLeftStart()
    {
        Debug.Log("Left button pressed");
        isCameraLeftFlg = true;
    }

    public void OnLeftEnd()
    {
        isCameraLeftFlg = false;
    }

    public void OnCurrentPosition()
    {
        worldMapCameraManager.ResetCamera(); // カメラの位置をリセット
    }
}
