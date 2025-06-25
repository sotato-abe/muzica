using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fieldPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 移動は押している間ずっとにする
        if (Input.GetKey(KeyCode.RightArrow))
        {
            MovePlayer(Vector3.right);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MovePlayer(Vector3.left);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            MovePlayer(Vector3.up);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            MovePlayer(Vector3.down);
        }
    }

    // プレイヤーの移動処理
    void MovePlayer(Vector3 direction)
    {
        // プレイヤーの現在位置を取得
        Vector3 currentPosition = transform.position;

        // 新しい位置を計算
        Vector3 newPosition = currentPosition + direction;

        // 新しい位置にプレイヤーを移動
        transform.position = newPosition;
    }    
}
