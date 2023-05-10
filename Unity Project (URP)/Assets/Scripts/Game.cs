using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
    private AudioManager _audioManager;

    void Awake()
    {
        _audioManager = FindObjectOfType<AudioManager>();

        _audioManager.Stop("Main Menu");
        _audioManager.Play("Game");
    }

    // private IEnumerator StartCountdown()
    // {
 
    // }
}
