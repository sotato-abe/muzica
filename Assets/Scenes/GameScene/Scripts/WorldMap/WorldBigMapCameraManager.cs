using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBigMapCameraManager : MonoBehaviour
{
    private int minXposition = -50; // ワールドマップのXの最小位置
    private int maxXposition = -37; // ワールドマップのXの最大
    private int minYposition = -45; // ワールドマップのYの最小位置
    private int maxYposition = -35; // ワールドマップのYの最大位置

    private Vector3 currentPos; // 現在のカメラ位置
    private Vector3 targetPos; // 目標のカメラ位置

    float moveSpeed = 0.7f; // カメラの移動速度
    float smoothSpeed = 8f; // カメラの移動のなめらかさ

    // WorldMapのPlayer位置にカメラを合わせる
    public void TargetPlayer(Vector3 newPos)
    {
        currentPos = newPos; // 現在のカメラ位置を更新
        targetPos = newPos; // 目標のカメラ位置を更新
    }

    // WorldMapのPlayer位置にカメラを合わせる
    public void ChangeTarget(Vector3 newPos)
    {
        // 目標となるFieldの位置にカメラを移動
        targetPos = newPos;
    }

    void LateUpdate()
    {
        if (transform.position == targetPos)
            return;

        SetCameraPosition(); // カメラの位置を更新
    }

    public void ResetCamera()
    {
        targetPos = currentPos; // 目標のカメラ位置を更新
    }

    public void UpTarget()
    {
        // カメラの位置を上に移動
        Vector3 cameraPos = transform.position;
        cameraPos.y += moveSpeed; // 上に1ユニット移動
        if (cameraPos.y > maxYposition) // 上限を超えないように制限
            return;

        targetPos = cameraPos;
    }

    public void DownTarget()
    {
        // カメラの位置を下に移動
        Vector3 cameraPos = transform.position;
        cameraPos.y -= moveSpeed; // 下に1ユニット移動
        if (cameraPos.y < minYposition) // 下限を超えないように制限
            return;

        targetPos = cameraPos;
    }

    public void RightTarget()
    {
        // カメラの位置を右に移動
        Vector3 cameraPos = transform.position;
        cameraPos.x += moveSpeed; // 右に1ユニット移動
        if (cameraPos.x > maxXposition) // 右限を超えないように制限
            return;

        targetPos = cameraPos;
    }

    public void LeftTarget()
    {
        // カメラの位置を左に移動
        Vector3 cameraPos = transform.position;
        cameraPos.x -= moveSpeed; // 左に1ユニット移動
        if (cameraPos.x < minXposition) // 左限を超えないように制限
            return;

        targetPos = cameraPos;
    }

    private void SetCameraPosition()
    {
        targetPos.z = transform.position.z;

        if (Vector3.Distance(transform.position, targetPos) < 0.001f)
        {
            transform.position = targetPos; // 誤差吸収してピタッと固定
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
        }
    }
}