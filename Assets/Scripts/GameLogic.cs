using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.VFX;
using System.Collections.Generic;
using System.Linq;


public class GameLogic : MonoBehaviour
{
    public GameObject counter; // Counter object for displaying 8/8
    public int pageCount; // Count of collected pages
    public GameObject vinesEffectObject;

    private bool isRotating = false; // To track camera rotation
    public float faceOffsetY = 2.5f; // Offset to look at Slenderman's face

    //For the Prefab pages added spots thing
    public GameObject[] pagePrefabs; // Assign your page prefab in the inspector
    public Transform[] spawnPoints; // Assign your spawn points in the inspector
    public int totalPages = 8; // Number of pages to spawn

    private List<int> usedSpawnPoints = new List<int>(); // To track used spawn points

    public bool isDead = false; //track if player dead i wanna find in other scripts


    void Start()
    {
        pageCount = 0; // Initialize page count
        SpawnPages();

    }

    void Update()
    {
        // Update the counter display
        counter.GetComponent<TextMeshProUGUI>().text = pageCount + "/8";
    }

    void SpawnPages()
    {
        // Ensure we don't spawn more pages than we have spawn points
        if (spawnPoints.Length < pagePrefabs.Length || spawnPoints.Length < totalPages)
        {
            Debug.LogError("Not enough spawn points for the total number of unique pages!");
            return;
        }

        // Create a list of all spawn point indices
        List<int> availableSpawnPoints = new List<int>();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            availableSpawnPoints.Add(i);
        }

        // Shuffle the page prefabs to assign each one randomly
        GameObject[] shuffledPages = pagePrefabs.OrderBy(x => Random.value).ToArray();

        int pagesSpawned = 0;

        while (pagesSpawned < totalPages)
        {
            // Pick a random index from the available spawn points
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            int spawnPointIndex = availableSpawnPoints[randomIndex];

            // Spawn the current page at the chosen spawn point
            Instantiate(shuffledPages[pagesSpawned], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);

            // Remove this spawn point from the list of available spawn points
            availableSpawnPoints.RemoveAt(randomIndex);

            pagesSpawned++;
        }
    }

    public void HandlePlayerDeath()
    {
        Debug.Log("GameOver");

        isDead = true;

        // Find the player object
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            return;
        }

        // Disable PlayerMovement script
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false; // Disable player movement
        }

        // Find Slenderman object
        GameObject slendermanObject = GameObject.FindWithTag("Slenderman");
        if (slendermanObject == null)
        {
            return;
        }

        // Smoothly rotate camera to look at Slenderman's face
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Transform cameraTransform = mainCamera.transform;
            if (!isRotating)
            {
                StartCoroutine(SmoothLookAtAndTriggerVFX(cameraTransform, slendermanObject.transform));
            }
        }
    }

    private IEnumerator SmoothLookAtAndTriggerVFX(Transform cameraTransform, Transform targetTransform)
    {
        isRotating = true;

        // Duration of the rotation
        float duration = 1.5f;
        float elapsedTime = 0f;

        // Store the initial rotation of the camera
        Quaternion initialRotation = cameraTransform.rotation;

        // Calculate the target position with upward offset
        Vector3 targetPosition = targetTransform.position + new Vector3(0, faceOffsetY, 0);

        // Calculate the target rotation
        Vector3 directionToTarget = (targetPosition - cameraTransform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        while (elapsedTime < duration)
        {
            // Interpolate the rotation smoothly
            cameraTransform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / duration);

            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the camera ends at the exact target rotation
        cameraTransform.rotation = targetRotation;

        isRotating = false;

        // Trigger the VFX on Slenderman
        TriggerVFX();

        yield return new WaitForSeconds(3f);

        DisableAllSounds();
    }

    private void TriggerVFX()
    {
        // Find the VFX particle system by tag

        if (vinesEffectObject != null)
        {
            vinesEffectObject.SetActive(true);
        }
    }

    private void DisableAllSounds()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.enabled = false;
        }
    }
}
