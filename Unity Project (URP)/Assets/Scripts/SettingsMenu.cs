using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    private Slider _musicSlider;
    
    [SerializeField]
    private Slider _soundSlider;

    [SerializeField]
    private AudioMixer _masterMixer;

    void Awake()
    {
        if(!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 0);
        }

        if(!PlayerPrefs.HasKey("SoundVolume"))
        {
            PlayerPrefs.SetFloat("SoundVolume", 0);
        }

        LoadVolume();
    }

    public void UpdateMusicVolume()
    {
        float volume = _musicSlider.value;

        PlayerPrefs.SetFloat("MusicVolume", volume);
        _masterMixer.SetFloat("MusicVolume", volume);
    }

    public void UpdateSoundVolume()
    {
        float volume = _soundSlider.value;

        PlayerPrefs.SetFloat("SoundVolume", volume);
        _masterMixer.SetFloat("SoundVolume", volume);
    }

    public void ResetHighscore()
    {
        PlayerPrefs.SetInt("Highscore", 0);
    }

    private void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        float soundVolume = PlayerPrefs.GetFloat("SoundVolume");

        _masterMixer.SetFloat("MusicVolume", musicVolume);
        _musicSlider.value = musicVolume;

        _masterMixer.SetFloat("SoundVolume", soundVolume);
        _soundSlider.value = soundVolume;
    }
}
