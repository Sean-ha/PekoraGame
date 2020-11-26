using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweenIn : MonoBehaviour
{
    public TweenType type;

    [System.Serializable]
    public enum TweenType
    {
        Scale,
        Fade
    }

    private void Start()
    {
        // Only buttons fade
        if(type == TweenType.Fade)
        {
            LeanTween.alpha(GetComponent<RectTransform>(), 0, 0);
            StartCoroutine(BeginButtonFade());
        }
        // Only the title scales
        if(type == TweenType.Scale)
        {
            LeanTween.scale(GetComponent<RectTransform>(), new Vector3(0, 0, 0), 0);
            LeanTween.scale(GetComponent<RectTransform>(), new Vector3(1.1f, 1.1f, 0), .3f).setOnComplete(Shrink);
        }
    }

    private IEnumerator BeginButtonFade()
    {
        yield return new WaitForSeconds(1.1f);
        LeanTween.alpha(GetComponent<RectTransform>(), 1, 1).setOnComplete(ActivateButton);
    }

    private void Shrink()
    {
        LeanTween.scale(GetComponent<RectTransform>(), new Vector3(1, 1, 0), 0.05f);
    }

    private void ActivateButton()
    {
        GetComponent<Button>().interactable = true;
    }
}
