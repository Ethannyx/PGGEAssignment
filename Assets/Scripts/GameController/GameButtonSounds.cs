using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameButtonSounds : MonoBehaviour
{
    public AudioClip MenuSound;
    public AudioClip EnterSound;
    public AudioClip ExitSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPress()
    {
        if (gameObject.CompareTag("Menu"))
        {
            //Play sound if menu button
            PlaySound(MenuSound);
        }
        else if (gameObject.CompareTag("Enter"))
        {
            //Play sound if enter button
            PlaySound(EnterSound);
        }
        else if (gameObject.CompareTag("Exit"))
        {
            //Play sound if exit button
            PlaySound(ExitSound);
        }
    }

    void PlaySound(AudioClip sound)
    {
        
        if (sound != null)
        {
            audioSource.PlayOneShot(sound);
        }
        else
        {
            Debug.LogWarning("No sound provided.");
        }
    }
}
