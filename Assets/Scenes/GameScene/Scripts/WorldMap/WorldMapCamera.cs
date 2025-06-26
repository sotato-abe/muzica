using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapCamera : MonoBehaviour
{
    private int worldHeight = 100; // ワールドマップの高さ

    private Vector3Int currentPos; // 現在のカメラ位置

    // WorldMapのPlayer位置にカメラを合わせる
    public void TargetPlayer(Vector3Int targetPos)
    {
        currentPos = targetPos; // 現在のカメラ位置を更新
        // カメラの位置をターゲットの位置に合わせる
        Vector3 cameraPos = new Vector3(20, targetPos.y, -10); // Z軸は-10に固定
        transform.position = cameraPos;
    }

    public void ResetCamera()
    {
        TargetPlayer(currentPos); // 現在のカメラ位置に戻す
    }

    public void UpTarget()
    {
        // カメラの位置を上に移動
        Vector3 cameraPos = transform.position;
        cameraPos.y += 0.05f; // 上に1ユニット移動
        if (cameraPos.y > worldHeight) // 上限を超えないように制限
        {
            cameraPos.y = 0;
        }
        transform.position = cameraPos;
    }

    public void DownTarget()
    {
        // カメラの位置を下に移動
        Vector3 cameraPos = transform.position;
        cameraPos.y -= 0.05f; // 下に1ユニット移動
        if (cameraPos.y < 0) // 下限を超えないように制限
        {
            cameraPos.y = worldHeight;
        }
        transform.position = cameraPos;
    }
}