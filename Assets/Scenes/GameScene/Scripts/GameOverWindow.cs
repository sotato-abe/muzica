using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameOverWindow : MonoBehaviour
{
    private void Start()
    {
        // 初期化処理
        Hide();
    }
    public void Show()
    {
        // ゲームオーバーウィンドウを表示する処理
        gameObject.SetActive(true);

    }
    public void Hide()
    {
        // ゲームオーバーウィンドウを非表示にする処理
        gameObject.SetActive(false);
    }

    // menuButtonのクリックイベントを設定
    // メニューのシーンへ移動する
    public void GameMenu()
    {
        SceneManager.LoadScene("StartScene");
    }
}
