using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Diagnostics;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance; // makes sure there is only 1 AudioManager

    public void Awake() // set up everything in Awake to play sounds at the start
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds) // for every sound in our array an AudioSource gets added
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        // StartCoroutine(PlayIntroThenLoop());
    }

    //private IEnumerator PlayIntroThenLoop()
    //{
    //    Sound intro = Array.Find(sounds, sound => sound.name == "ThemeIntro");
    //    Play("ThemeIntro");
    //    yield return new WaitForSeconds(intro.source.clip.length); // Wait for ThemeIntro to finish
    //    Play("ThemeLoop"); // Play ThemeLoop immediately after ThemeIntro finishes
    //}

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) // don't play a sound that doesn't exist
        {
            return;
        }
        s.source.Play();
    }
}
