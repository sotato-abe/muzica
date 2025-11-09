using System.Collections;
using UnityEngine;

public class TreasureBoxPrefab : FieldTriggerPrefab
{
    public override void EnterAction()
    {
        FieldController.Instance.OpenTreasureBox();
        StartCoroutine(OpenMotion());}

    private IEnumerator OpenMotion()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("isOpen");
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
