using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public PlayerController pc;
    public ScoreManager scoreManager;
    public Image blackScreen;
    public Image optionsPanel;
    public Image statsPanel;
    public Image aboutPanel;
    public bool mainMenu;
    public List<Button> pauseButtons;

    private AudioSource bgmSource;
    private Button[] buttons;

    private void Start()
    {
        bgmSource = GameObject.Find("BGM").GetComponent<AudioSource>();
        buttons = FindObjectsOfType<Button>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && mainMenu)
        {
            if(aboutPanel.gameObject.activeInHierarchy)
            {
                CloseAbout();
            }
            else if(optionsPanel.gameObject.activeInHierarchy)
            {
                CloseOptions();
            }
            else if(statsPanel.gameObject.activeInHierarchy)
            {
                CloseStats();
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
        // SoundManager.instance.PlaySound(SoundManager.Sound.Click);

        int rand = Random.Range(0, 2);
        if(rand == 0)
        {
            SoundManager.instance.PlaySound(SoundManager.Sound.LetsGo);
        }
        else if(rand == 1)
        {
            SoundManager.instance.PlaySound(SoundManager.Sound.Ikuyo);
        }
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
        SoundManager.instance.PlaySound(SoundManager.Sound.Pekopeko);
        //SoundManager.instance.PlaySound(SoundManager.Sound.Click);
        optionsPanel.gameObject.SetActive(true);
    }

    public void PressStatsButton()
    {
        SoundManager.instance.PlaySound(SoundManager.Sound.Peko4);
        //SoundManager.instance.PlaySound(SoundManager.Sound.Click);
        statsPanel.gameObject.SetActive(true);
    }

    public void PressRetryButton()
    {
        DisableButtons();
        LeanTween.alpha(blackScreen.rectTransform, 1, .75f).setOnComplete(GoToGameScene);
        SoundManager.instance.PlaySound(SoundManager.Sound.Peko5);
    }

    public void PressMainMenuButton()
    {
        DisableButtons();
        LeanTween.alpha(blackScreen.rectTransform, 1, .75f).setOnComplete(GoToMainMenu);
        SoundManager.instance.PlaySound(SoundManager.Sound.Pekopeko);
    }

    public void OpenLinkJSPlugin()
    {
        #if !UNITY_EDITOR
		openWindow("https://twitter.com/intent/tweet?text=Scored " + Mathf.FloorToInt(scoreManager.GetScore()) +
            " points in the game 'Return To Pekoland'!%0a" +
            "%23ReturnToPekoland%0a" +
            "https://seanshome.itch.io/return-to-pekoland %0a");
        #endif
    }

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void openWindow(string url);

    public void CloseOptions()
    {
        SoundManager.instance.PlaySound(SoundManager.Sound.Click);
        optionsPanel.gameObject.SetActive(false);
    }

    public void CloseStats()
    {
        SoundManager.instance.PlaySound(SoundManager.Sound.Click);
        statsPanel.gameObject.SetActive(false);
    }

    public void OpenAboutPanel()
    {
        SoundManager.instance.PlaySound(SoundManager.Sound.Peko4);
        aboutPanel.gameObject.SetActive(true);
    }

    public void CloseAbout()
    {
        SoundManager.instance.PlaySound(SoundManager.Sound.Click);
        aboutPanel.gameObject.SetActive(false);
    }

    public void OpenTwitterProfile()
    {
        #if !UNITY_EDITOR
		openWindow("https://twitter.com/SeansHome");
        #endif
    }

    public void LaughButton()
    {
        SoundManager.instance.PlaySound(SoundManager.Sound.Click);
        SoundManager.instance.PlaySound(SoundManager.Sound.Laugh);
    }

    public void PressExitButton()
    {
        pc.SetClosingPauseMenu(true);
        foreach (Button b in pauseButtons)
        {
            b.interactable = false;
        }
        SoundManager.instance.PlaySound(SoundManager.Sound.Click);
        LeanTween.alpha(blackScreen.rectTransform, 1, 0.5f).setIgnoreTimeScale(true).setOnComplete(GoToMainMenuAndResume);
        void GoToMainMenuAndResume()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
    }
}
