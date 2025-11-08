using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class FieldCharacter : MonoBehaviour
{
    public Animator animator;
    [SerializeField] SpriteRenderer CharacterImage; // スプライトイメージパネル
    [SerializeField] public SpriteRenderer IconImage; // スプライトイメージパネル
    [SerializeField] public SpriteRenderer IconBackImage; // スプライトイメージパネル

    // TODO characterTypeをenumで管理してここで渡してスプライトを切り替える
    public void SetUp()
    {
        StartCoroutine(JumpMotion());
    }

    public void Inversion(bool isLeft)
    {
        CharacterImage.flipX = isLeft; // キャラクターのスプライトを反転
    }

    public virtual void SetAnimation(AnimationType animationType)
    {
        UnityEngine.Debug.Log($"Setting animation: {animationType}");
        switch (animationType)
        {
            case AnimationType.Attack:
                animator.SetTrigger("Attack");
                break;
            case AnimationType.Damage:
                animator.SetTrigger("Damage");
                break;
            case AnimationType.Recovery:
                animator.SetTrigger("Recovery");
                break;
            case AnimationType.Buff:
                animator.SetTrigger("Buff");
                break;
            case AnimationType.Debuff:
                animator.SetTrigger("Debuff");
                break;
            case AnimationType.Death:
                animator.SetTrigger("Death");
                break;
            default:
                UnityEngine.Debug.LogWarning($"Unknown animation type: {animationType}");
                break;
        }
    }

    public IEnumerator JumpMotion()
    {
        float bounceHeight = 0.7f;
        float damping = 0.7f;
        float gravity = 300f;
        float groundY = transform.position.y;

        while (bounceHeight >= 0.1f)
        {
            float verticalVelocity = Mathf.Sqrt(2 * gravity * bounceHeight);
            bool isFalling = false;

            // 上昇と下降のループ
            while (transform.position.y >= groundY || !isFalling)
            {
                verticalVelocity -= gravity * Time.deltaTime;
                transform.position += Vector3.up * verticalVelocity * Time.deltaTime;

                if (transform.position.y <= groundY)
                {
                    isFalling = true;
                    break;
                }

                yield return null;
            }

            bounceHeight *= damping;  // バウンドを減衰させる
        }

        transform.position = new Vector3(transform.position.x, groundY, transform.position.z);  // 最後に位置を調整
    }
}
