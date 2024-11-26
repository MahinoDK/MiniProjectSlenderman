using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    //5 variables
    public GameObject flashlight;

    public AudioSource turnOn;
    public AudioSource turnOff;

    private bool on;
    private bool off;

    //for tilting when running
    public float tiltAmount = 40f; //tilt when running
    public float tiltSpeed = 5f; //speed of the tilt animation

    private Quaternion initialRotation; //store starting position of flashlight
    private Quaternion runningRotation;
    private PlayerMovement playerMovement; //reference to movement script

    private void Start()
    {
        on = true;
        flashlight.SetActive(true);


        // Store the initial rotation of the flashlight
        initialRotation = flashlight.transform.localRotation;

        // Calculate the downward tilt for running
        runningRotation = initialRotation * Quaternion.Euler(tiltAmount, 0, 0);

        // Find the PlayerMovement component in the parent or other related object
        playerMovement = GetComponentInParent<PlayerMovement>();

    }

    private void Update()
    {
        if (off && Input.GetButtonDown("flashlight"))
        {
            flashlight.SetActive(true);
            turnOn.Play();
            off = false;
            on = true;
        }
        else if (on && Input.GetButtonDown("flashlight"))
        {
            flashlight.SetActive(false);
            turnOff.Play();
            off = true;
            on = false;
        }

        // Adjust flashlight tilt based on whether the player is running
        if (playerMovement != null && Input.GetKey(KeyCode.LeftShift) && playerMovement.characterController.velocity.magnitude > 0)
        {
            // Smoothly tilt the flashlight downward while running
            flashlight.transform.localRotation = Quaternion.Lerp(flashlight.transform.localRotation, runningRotation, Time.deltaTime * tiltSpeed);
        }
        else
        {
            // Smoothly return the flashlight to its initial rotation
            flashlight.transform.localRotation = Quaternion.Lerp(flashlight.transform.localRotation, initialRotation, Time.deltaTime * tiltSpeed);
        }
    }
}
