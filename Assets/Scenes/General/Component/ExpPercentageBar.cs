using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpPercentageBar : PercentageBar
{
    public void SetExpBar(int levelDifference, int exp)
    {
        currentPercentage = (float)exp / 100f;
        StartCoroutine(PlayLevelUpAnimation(levelDifference));
    }

    private IEnumerator PlayLevelUpAnimation(int levelDifference)
    {
        for (int i = 0; i < levelDifference; i++)
        {
            yield return StartCoroutine(FullBar()); // フルまで行って…
            ResetBar();                     // 0に戻す（レベルアップ演出）
            yield return null;
        }

        ScrollBarImmediate();
    }
}
