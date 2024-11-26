using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioClip soundEffect; //reffeence sound
    private AudioSource audioSource;
    public float minInterval = 2; //start after minimun 2 seconds
    public float maxInterval = 40; //start after max 40 seconds

    private float timer;
    private float nextSoundTime;

    private void Start()
    {
        //access or add audiosource here
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        //set the sound effect
        audioSource.clip = soundEffect;
        //then initialize the timer to be random in range between our intervals we chose
        nextSoundTime = Random.Range(minInterval, maxInterval);
    }

    private void Update()
    {
        //update the timer
        timer += Time.deltaTime;

        //checking if it is time to play the sound
        if (timer >= nextSoundTime) 
        {
            PlayRandomSound();

            //resetting timer and set new random interval
            timer = 0f;
            nextSoundTime = Random.Range(minInterval, maxInterval);
        }
    }

    private void PlayRandomSound()
    {
        //Playing one of the soundeffects
        audioSource.Play();
    }
}
