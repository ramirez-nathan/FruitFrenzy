using UnityEngine.Audio;
using UnityEngine;

[System.Serializable] // allows this custom class to appear in the inspector
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;

    // allow mocking for testing
    public virtual void Play()
    {
        source.Play();
    }
}
