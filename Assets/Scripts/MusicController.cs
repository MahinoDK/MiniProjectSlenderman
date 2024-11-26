using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource; // AudioSource component for music
    public AudioClip music1; // Music when first page is collected
    public AudioClip music2; // Music when third page is collected
    public AudioClip music3; // Music when fifth page is collected
    public AudioClip music4; // Music when seventh page is collected
    private int currentMusicIndex = -1; // Track which music is currently playing
    private GameLogic gameLogic; // Reference to GameLogic

    private void Start()
    {
        // Set up AudioSource if not assigned
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true; // Loop the music
            audioSource.playOnAwake = false; // Prevent music from playing at the start
        }

        // Find GameLogic in the scene to access page count
        gameLogic = GameObject.FindWithTag("GameLogic").GetComponent<GameLogic>();
    }

    private void Update()
    {
        int pageCount = gameLogic.pageCount;

        // Change music based on page count milestones
        if (pageCount == 1 && currentMusicIndex != 0)
        {
            PlayMusic(0); // Play the first music when the first page is collected
            Debug.Log("first Music");
        }
        else if (pageCount == 3 && currentMusicIndex != 1)
        {
            PlayMusic(1); // Play the second music when the third page is collected
            Debug.Log("Second Music");

        }
        else if (pageCount == 5 && currentMusicIndex != 2)
        {
            PlayMusic(2); // Play the third music when the fifth page is collected
            Debug.Log("Third Music");

        }
        else if (pageCount == 7 && currentMusicIndex != 3)
        {
            PlayMusic(3); // Play the fourth music when the seventh page is collected
            Debug.Log("Fourhts Music");

        }
    }

    private void PlayMusic(int musicIndex)
    {
        currentMusicIndex = musicIndex;

        // Assign the appropriate music clip
        switch (musicIndex)
        {
            case 0:
                audioSource.clip = music1;
                break;
            case 1:
                audioSource.clip = music2;
                break;
            case 2:
                audioSource.clip = music3;
                break;
            case 3:
                audioSource.clip = music4;
                break;
        }

        // Start playing the new music
        audioSource.Play();
    }
}