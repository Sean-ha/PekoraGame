using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsSliders : MonoBehaviour
{
    public Text sliderValueText;
    public SliderType sliderType;
    public AudioSource bgmSource;

    private List<AudioSource> audioSources;

    private string prefsKey;

    [System.Serializable]
    public enum SliderType
    {
        BGM,
        SFX
    }

    private void Start()
    {
        audioSources = new List<AudioSource>();
        if(sliderType == SliderType.BGM)
        {
            audioSources.Add(bgmSource);
            prefsKey = "BGMVol";
        }
        else
        {
            prefsKey = "SFXVol";
            GameObject[] sfxObjs = GameObject.FindGameObjectsWithTag("SoundEffect");
            foreach(GameObject go in sfxObjs)
            {
                audioSources.Add(go.GetComponent<AudioSource>());
            }
        }
        GetComponent<Slider>().value = PlayerPrefs.GetFloat(prefsKey, 50);
    }

    public void OnSliderChanged(float value)
    {
        sliderValueText.text = value + "%";
        foreach(AudioSource a in audioSources)
        {
            a.volume = value / 100;
        }

        PlayerPrefs.SetFloat(prefsKey, value);
    }
}
