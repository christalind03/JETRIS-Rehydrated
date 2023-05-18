using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private Music[] _allMusic;

    [SerializeField]
    private Sound[] _allSounds;

    [SerializeField]
    private AudioMixer _masterMixer;

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

        // Load music and sounds
        foreach(Music music in _allMusic)
        {
            music.source = gameObject.AddComponent<AudioSource>();

            music.source.clip = music.audioClip;
            music.source.volume = music.volume;
            music.source.loop = music.loop;

            music.source.outputAudioMixerGroup = _masterMixer.FindMatchingGroups("Music Volume")[0];
        }
        
        foreach(Sound sound in _allSounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.audioClip;
            sound.source.volume = sound.volume;
            sound.source.loop = sound.loop;

            sound.source.outputAudioMixerGroup = _masterMixer.FindMatchingGroups("Sound Volume")[0];
        }
    }

    public void PlayMusic(string name)
    {
        Music selectedMusic = Array.Find(_allMusic, music => music.name == name);

        if(selectedMusic == null)
        {
            Debug.LogWarning("The name \"" + name + "\" could not be found as a Music object.");
            return;
        }
        
        selectedMusic.source.Play();
    }

    public void PlaySound(string name)
    {
        Sound selectedSound = Array.Find(_allSounds, sound => sound.name == name);
        
        if(selectedSound == null)
        {
            Debug.LogWarning("The name \"" + name + "\" could not be found as a Sound object.");
            return;
        }

        selectedSound.source.Play();
    }

    public void StopMusic(string name)
    {
        Music selectedMusic = Array.Find(_allMusic, music => music.name == name);

        if(selectedMusic == null)
        {
            Debug.LogWarning("The name \"" + name + "\" could not be found as a Music object.");
            return;
        }
        
        selectedMusic.source.Stop();
    }

    public void StopSound(string name)
    {
        Sound selectedSound = Array.Find(_allSounds, sound => sound.name == name);
        
        if(selectedSound == null)
        {
            Debug.LogWarning("The name \"" + name + "\" could not be found as a Sound object.");
            return;
        }

        selectedSound.source.Stop();
    }
}

[System.Serializable]
public class Music
{
    public string name;
    public AudioClip audioClip;

    [Range(0f, 1f)]
    public float volume;
    public bool loop;

    [HideInInspector]
    public AudioSource source;
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