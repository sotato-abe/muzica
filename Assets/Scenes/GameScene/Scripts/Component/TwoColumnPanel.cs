using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TwoColumnPanel : MonoBehaviour
{
    [SerializeField] SlidePanel playerWindow;
    [SerializeField] SlidePanel ownerWindow;

    public virtual void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetActive(bool isActive)
    {
        if (isActive)
        {
            this.gameObject.SetActive(true);
            playerWindow.SetActive(true);
            ownerWindow.SetActive(true);
        }
        else
        {
            playerWindow.SetActive(false);
            ownerWindow.SetActive(false);
        }
    }
}
