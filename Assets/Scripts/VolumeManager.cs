using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    public AudioSource bgmSource;

    private void Start()
    {
        float sfxVol = PlayerPrefs.GetFloat("SFXVol", 50) / 100;

        GameObject[] sfxObjs = GameObject.FindGameObjectsWithTag("SoundEffect");
        foreach (GameObject go in sfxObjs)
        {
            go.GetComponent<AudioSource>().volume = sfxVol;
        }

        bgmSource.volume = PlayerPrefs.GetFloat("BGMVol", 50) / 100;
    }
}
