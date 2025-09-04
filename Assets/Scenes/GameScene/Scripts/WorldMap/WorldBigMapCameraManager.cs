using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBigMapCameraManager : MonoBehaviour
{
    private int minXposition = -55; // ワールドマップのXの最小位置
    private int maxXposition = -33; // ワールドマップのXの最大
    private int minYposition = 4; // ワールドマップのYの最小位置
    private int maxYposition = 17; // ワールドマップのYの最大位置

    private Vector3 currentPos; // 現在のカメラ位置

    // WorldMapのPlayer位置にカメラを合わせる
    public void TargetPlayer(Vector3 targetPos)
    {
        currentPos = targetPos; // 現在のカメラ位置を更新
        transform.position = currentPos;
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
        if (cameraPos.y > maxYposition) // 上限を超えないように制限
            return;

        transform.position = cameraPos;
    }

    public void DownTarget()
    {
        // カメラの位置を下に移動
        Vector3 cameraPos = transform.position;
        cameraPos.y -= 0.05f; // 下に1ユニット移動
        if (cameraPos.y < minYposition) // 下限を超えないように制限
            return;

        transform.position = cameraPos;
    }

    public void RightTarget()
    {
        // カメラの位置を右に移動
        Vector3 cameraPos = transform.position;
        cameraPos.x += 0.05f; // 右に1ユニット移動
        if (cameraPos.x > maxXposition) // 右限を超えないように制限
            return;

        transform.position = cameraPos;
    }

    public void LeftTarget()
    {
        // カメラの位置を左に移動
        Vector3 cameraPos = transform.position;
        cameraPos.x -= 0.05f; // 左に1ユニット移動
        if (cameraPos.x < minXposition) // 左限を超えないように制限
            return;

        transform.position = cameraPos;
    }
}