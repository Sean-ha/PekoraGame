using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public Image blackScreen;

    private Button[] buttons;

    private void Start()
    {
        buttons = FindObjectsOfType<Button>();
    }

    public void PressPlayButton()
    {
        foreach(Button b in buttons)
        {
            b.interactable = false;
        }
        LeanTween.alpha(blackScreen.rectTransform, 1, .75f).setOnComplete(GoToGameScene);
    }

    private void GoToGameScene()
    {
        SceneManager.LoadScene(1);
    }
}
