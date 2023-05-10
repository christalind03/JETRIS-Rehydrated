using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private Sound[] allSounds;

    public static AudioManager instance;

    void Awake()
    {
        // Enable singleton
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // Load sounds
        foreach(Sound sound in allSounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.audioClip;
            sound.source.volume = sound.volume;
            sound.source.loop = sound.loop;

        }
    }

    public void Play(string name)
    {
        Sound selectedSound = Array.Find(allSounds, sound => sound.name == name);

        if(selectedSound == null)
        {
            Debug.LogWarning("Sound \"" + selectedSound.name + "\" could not be found");
            return;
        }

        selectedSound.source.Play();
    }

    public void Stop(string name)
    {
        Sound selectedSound = Array.Find(allSounds, sound => sound.name == name);

        if(selectedSound == null)
        {
            Debug.LogWarning("Sound \"" + selectedSound.name + "\" could not be found");
            return;
        }

        selectedSound.source.Stop();
    }
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip audioClip;

    [Range(0f, 1f)]
    public float volume;
    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
