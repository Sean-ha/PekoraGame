using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public Image blackScreen;
    public Image optionsPanel;
    public Image statsPanel;

    private AudioSource bgmSource;
    private Button[] buttons;

    private void Start()
    {
        bgmSource = GameObject.Find("BGM").GetComponent<AudioSource>();
        buttons = FindObjectsOfType<Button>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(optionsPanel.gameObject.activeInHierarchy)
            {
                SoundManager.instance.PlaySound(SoundManager.Sound.Click);
                optionsPanel.gameObject.SetActive(false);
            }
            else if(statsPanel.gameObject.activeInHierarchy)
            {
                SoundManager.instance.PlaySound(SoundManager.Sound.Click);
                statsPanel.gameObject.SetActive(false);
            }
        }
    }

    private void DisableButtons()
    {
        foreach (Button b in buttons)
        {
            b.interactable = false;
        }
    }

    public void PressPlayButton()
    {
        SoundManager.instance.PlaySound(SoundManager.Sound.Click);
        DisableButtons();
        LeanTween.alpha(blackScreen.rectTransform, 1, .75f).setOnComplete(GoToGameScene);
        StartCoroutine(FadeOut(bgmSource, 0.75f));
    }
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
    }

    private void GoToGameScene()
    {
        SceneManager.LoadScene(1);
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PressOptionsButton()
    {
        SoundManager.instance.PlaySound(SoundManager.Sound.Click);
        optionsPanel.gameObject.SetActive(true);
    }

    public void PressStatsButton()
    {
        SoundManager.instance.PlaySound(SoundManager.Sound.Click);
        statsPanel.gameObject.SetActive(true);
    }

    public void PressRetryButton()
    {
        DisableButtons();
        LeanTween.alpha(blackScreen.rectTransform, 1, .75f).setOnComplete(GoToGameScene);
        SoundManager.instance.PlaySound(SoundManager.Sound.Click);
    }

    public void PressMainMenuButton()
    {
        DisableButtons();
        LeanTween.alpha(blackScreen.rectTransform, 1, .75f).setOnComplete(GoToMainMenu);
        SoundManager.instance.PlaySound(SoundManager.Sound.Click);
    }
}
