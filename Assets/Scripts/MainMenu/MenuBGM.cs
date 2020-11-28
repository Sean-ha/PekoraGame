using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBGM : MonoBehaviour
{
    private AudioSource bgmSource;

    private void Awake()
    {
        bgmSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        int rand = Random.Range(0, 2);
        if(rand == 0)
        {
            SoundManager.instance.PlaySound(SoundManager.Sound.ItsMePekora);
        }
        else if(rand == 1)
        {
            SoundManager.instance.PlaySound(SoundManager.Sound.Konpeko);
        }
        StartCoroutine(StartMusic());
    }

    private IEnumerator StartMusic()
    {
        yield return new WaitForSeconds(.6f);
        bgmSource.Play();
    }
}
