using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(AspectRatioFitter))]
public class AutoAspectFitter : MonoBehaviour
{
    private Image image;
    private AspectRatioFitter fitter;

    private void Awake()
    {
        image = GetComponent<Image>();
        fitter = GetComponent<AspectRatioFitter>();
    }

    private void Start()
    {
        ApplyAspect();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // エディタ上でもリアルタイム反映
        if (image == null) image = GetComponent<Image>();
        if (fitter == null) fitter = GetComponent<AspectRatioFitter>();
        ApplyAspect();
    }
#endif

    public void ApplyAspect()
    {
        if (image.sprite == null) return;

        float aspect = image.sprite.rect.width / image.sprite.rect.height;
        fitter.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
        fitter.aspectRatio = aspect;
    }

    public void SetSprite(Sprite newSprite)
    {
        image.sprite = newSprite;
        ApplyAspect();
    }
}
