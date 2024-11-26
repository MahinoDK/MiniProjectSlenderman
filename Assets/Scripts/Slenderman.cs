using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slenderman : MonoBehaviour
{
    public Transform player; // Reference to the player gameobject we will look for
    private float teleportRange = 30f; // Maximum distance to teleport to 
    private float teleportCooldown = 20f; // Cooldown time for when to teleport
    private float returnCooldown = 12f; // Time before returning to start spot
    [Range(0f, 1f)] public float chaseProbability = 0.7f; // Chance to chase the player
    public float rotationSpeed = 50f; // Rotation speed to make sure it always looks at player
    public AudioClip teleportSound; // Sound to play when Slenderman teleports
    private AudioSource audioSource; // Audio source for teleport sound

    public GameObject staticObject; // Static effect over the camera
    private float staticActivationRange = 12f; // Activation range for the static effect
    private bool isStaticActive = false; 
    private float fieldOfViewAngle = 45f; // Field of view angle for static activation

    public float drainRate = 10f; // Drain rate per second when looking at Slenderman
    private float rechargeRate = 2f; // Recharge rate per second when not looking
    private float maxPlayerHealth = 100f; // Maximum health of the player
    private float playerHealth; // Current health of the player

    private Vector3 baseTeleportSpot; // Initial spot where Slenderman will return to
    private float teleportTimer; // Timer for teleport cooldown
    private bool returningToBase; // Flag to indicate if Slenderman is returning to base

    private GameLogic gameLogic; // Reference to GameLogic script for game state

    private void Start()
    {
        baseTeleportSpot = transform.position; // The base spot is the initial local position of Slenderman
        teleportTimer = Time.time; // Set the initial teleport timer

        // Initialize player health
        playerHealth = maxPlayerHealth;

        // Get or add the audio source component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = teleportSound; // Set the teleport sound

        // Ensure static effect is off initially
        if (staticObject != null)
        {
            staticObject.SetActive(false);
        }

        // Find GameLogic object and get reference to GameLogic script
        GameObject gameLogicObject = GameObject.FindWithTag("GameLogic");
        if (gameLogicObject != null)
        {
            gameLogic = gameLogicObject.GetComponent<GameLogic>();
        }
    }

    private void Update()
    {
        if (player == null) // Return early if player is null
        {
            return;
        }

        AdjustAggressionBasedOnPages(); // Adjust aggression based on pages collected

        if (!isStaticActive)
        {
            teleportTimer -= Time.deltaTime; // Reduce teleport timer
        }

        // Check if teleport cooldown has expired
        if (teleportTimer <= 0f && !isStaticActive)
        {
            if (returningToBase)
            {
                TeleportToBaseSpot();
                teleportTimer = teleportCooldown; // Reset timer after teleporting to base
                returningToBase = false;
            }
            else
            {
                DecideTeleportAction(); // Decide whether to teleport near player or to base
                teleportTimer = teleportCooldown; // Reset timer after teleporting
            }
        }

        RotateTowardsPlayer(); // Rotate Slenderman to face the player
        CheckStaticEffect(); // Check conditions to activate or deactivate static effect

        // Debugging: Log current health
        Debug.Log("Player Health: " + playerHealth);

        // Handle player death
        if (playerHealth <= 0)
        {
            PlayerDeath();
        }
    }

    private void AdjustAggressionBasedOnPages()
    {
        // Increase aggression based on number of pages collected
        int pageCollected = gameLogic.pageCount;

        // For every page collected, decrease cooldown, decrease range, and increase chase probability
        teleportCooldown = Mathf.Max(2f, 20f - pageCollected); // Minimum teleport cooldown of 2 seconds
        teleportRange = Mathf.Max(5f, 30f - pageCollected * 3f); // Maximum teleport range of 30 units
        chaseProbability = Mathf.Clamp(0.7f + pageCollected * 0.05f, 0.7f, 1f); // Maximum probability of 1
        drainRate = 10f + pageCollected * 5f; //Increase drainrate by 5f pr page collected

        // Debug logs for testing can be removed
        Debug.Log("Pages Collected: " + pageCollected);
        Debug.Log("Teleport Cooldown: " + teleportCooldown);
        Debug.Log("Drain Rate: " +  drainRate);
    }

    private void DecideTeleportAction()
    {
        float randomValue = Random.value;

        // Decide if Slenderman should chase player or return to base
        if (randomValue <= chaseProbability)
        {
            TeleportNearPlayer(); // Teleport near the player
        }
        else
        {
            TeleportToBaseSpot(); // Teleport back to base spot
            teleportTimer = returnCooldown; // Set cooldown timer for returning to base
        }
    }

    private void TeleportNearPlayer()
    {
        // Teleport to a random position within range near the player
        Vector3 randomPosition = player.position + Random.onUnitSphere * teleportRange;
        randomPosition.y = transform.position.y; // Keep the same y position
        transform.position = randomPosition; // Update position

        audioSource.Play(); // Play teleport sound
    }

    private void TeleportToBaseSpot()
    {
        // Return to the base teleport spot
        transform.position = baseTeleportSpot;
        returningToBase = true; // Set flag indicating Slenderman is at base

        audioSource.Play(); // Play teleport sound
        Debug.Log("Returned to Base");
    }

    private void RotateTowardsPlayer()
    {
        // Rotate Slenderman to face the player
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f; // Ignore vertical rotation

        if (directionToPlayer.sqrMagnitude > 0.01f) // Check for sufficient distance to avoid rotation issues when close
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void CheckStaticEffect()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool isClose = distanceToPlayer <= 5f; // Close proximity
        bool isInRange = distanceToPlayer <= staticActivationRange; // Within activation range
        bool isLookingAtSlenderman = false;

        if (isInRange)
        {
            // Check if the player is looking at Slenderman
            Vector3 directionToSlenderman = transform.position - player.position;
            float angle = Vector3.Angle(player.forward, directionToSlenderman);
            isLookingAtSlenderman = angle <= fieldOfViewAngle / 2;
        }

        // Activation conditions: close proximity OR looking within range
        bool shouldActivateStatic = isClose || isLookingAtSlenderman;

        //update the false to true depinging on
        isStaticActive = shouldActivateStatic;

        if (shouldActivateStatic)
        {
            if (staticObject != null && !staticObject.activeSelf)
            {
                staticObject.SetActive(true); // Activate static effect
            }

            // Health drain logic
            float drainMultiplier = isClose ? 2f : 1f; // Double drain when close
            playerHealth -= drainRate * drainMultiplier * Time.deltaTime;
            playerHealth = Mathf.Max(playerHealth, 0f); // Clamp health to minimum of 0
        }
        else
        {
            if (staticObject != null && staticObject.activeSelf)
            {
                staticObject.SetActive(false); // Deactivate static effect
            }

            // Health recharge logic
            playerHealth += rechargeRate * Time.deltaTime;
            playerHealth = Mathf.Min(playerHealth, maxPlayerHealth); // Clamp health to max
        }
    }



    private void PlayerDeath()
    {
        // Logic for player death
        Debug.Log("Player has died!");

        // Call GameLogic or other scripts to handle death
        if (gameLogic != null)
        {
            gameLogic.HandlePlayerDeath(); // Example method in GameLogic
        }
    }
}
