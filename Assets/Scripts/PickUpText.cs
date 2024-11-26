using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpText : MonoBehaviour
{
    public static PickUpText instance;
    public GameObject textObject;
    public AudioSource audioSource;


    void Awake()
    {
        instance = this;
    }

    public void EnableText(bool state)
    {
        textObject.SetActive(state);
    }

    public void PlaySound()
    {
        audioSource.Play();
    }
}
