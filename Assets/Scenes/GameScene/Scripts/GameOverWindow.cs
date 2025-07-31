using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameOverWindow : MonoBehaviour
{
    public void Show()
    {
        // ゲームオーバーウィンドウを表示する処理
        gameObject.SetActive(true);
    }

    // menuButtonのクリックイベントを設定
    // メニューのシーンへ移動する
    public void GameMenu()
    {
        SceneManager.LoadScene("StartScene");
    }
}
