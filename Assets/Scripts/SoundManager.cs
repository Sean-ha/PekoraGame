﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public enum Sound
    {
        CarrotPickUp,
        GoldenCarrot,
        Death,
        Laugh,
        Jump,
        Click
    }

    [System.Serializable]
    public class SoundAudioClip
    {
        public Sound sound;
        public AudioClip audioClip;
    }

    public List<SoundAudioClip> sounds;
    private Dictionary<Sound, AudioSource> dict;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            dict = new Dictionary<Sound, AudioSource>();
            InitializeDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeDictionary()
    {
        // Creates a dictionary for each audio source containing each different sound that can be played
        foreach(SoundAudioClip clip in sounds)
        {
            dict[clip.sound] = new GameObject("Sound").AddComponent<AudioSource>();
            DontDestroyOnLoad(dict[clip.sound]);
            dict[clip.sound].tag = "SoundEffect";
            dict[clip.sound].clip = clip.audioClip;
        }
    }

    public void PlaySound(Sound sound)
    {
        dict[sound].Play();
    }

    private AudioClip GetAudioClip(Sound sound)
    {
        foreach (SoundAudioClip soundClip in sounds)
        {
            if (soundClip.sound == sound)
            {
                return soundClip.audioClip;
            }
        }
        return null;
    }
}