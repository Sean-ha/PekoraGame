using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : MonoBehaviour
{
    public Image blackScreen;

    private void Start()
    {
        LeanTween.alpha(blackScreen.rectTransform, 1, 0);
        LeanTween.alpha(blackScreen.rectTransform, 0, .75f);
    }
}
