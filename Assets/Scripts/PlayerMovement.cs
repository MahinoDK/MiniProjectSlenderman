using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//add character movement controller
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    //Camera
    public Camera playerCam;

    //movement settings

    public float walkSpeed = 3f;
    public float runSpeed = 5f;
    public float gravity = 10f;

    //camera settings
    public float lookSpeed = 2f;
    public float lookXLimit = 75f; // vi vil ik kigge helt 90 straight op og ned og ikke tip baglæns

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    //camera zoom
    public int ZoomFOV = 35;
    public int initialFOV;
    public float cameraZoomSmooth = 1; //så den zoomer langsomt smooth ik snap 

    private bool isZoomed = false;

    //Can player move?
    private bool canMove = true;

    public CharacterController characterController;

    //sound effects;
    public AudioSource cameraZoomSound;
    //måske flashlight klik?
    public GameObject walkingSound;

    private void Start()
    {
        //ensure we are using chracter controller component

        characterController = GetComponent<CharacterController>();

        //lock and hide cursor

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        //Walking/running action

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0; //if not running play walkspeed
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0; //if running play runspeed
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if(moveDirection.magnitude > 0)
        {
            walkingSound.SetActive(true);

            // Adjust sound pitch based on running or walking
            AudioSource walkingAudioSource = walkingSound.GetComponent<AudioSource>();
            walkingAudioSource.pitch = isRunning ? 1.3f : 1.0f; // 1.5x speed for running, 1x for walking
        }
        else
        {
            walkingSound.SetActive(false);
        }


        if (characterController.isGrounded)
        {
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        }

        characterController.Move(moveDirection * Time.deltaTime);

        //camera movement in action

        if (canMove)
        {
            //move player with the camera make sure we always look forward
            rotationX -= Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        //zooming in action

        if (Input.GetButtonDown("Fire2")) //når du højreklikker zoom
        {
            isZoomed = true; //så zoomer man og spilelr lyd
            cameraZoomSound.Play();
        }

        if (Input.GetButtonUp("Fire2")) //ik trykker mere
        {
            isZoomed = false;
            cameraZoomSound.Play();
        }

        if(isZoomed) //vi ændrer fov til zoomFOV 
        {
            playerCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(playerCam.fieldOfView, ZoomFOV, Time.deltaTime * cameraZoomSmooth);
        }
        else if (!isZoomed) //når ik zoom har vi originale inital zoom prop
        {
            playerCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(playerCam.fieldOfView, initialFOV, Time.deltaTime * cameraZoomSmooth);
        }

    }
}
