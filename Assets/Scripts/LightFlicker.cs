using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light targetLight; //put light component in here
    public float minFlickerDelay = 0.1f; //min time between flicekrs
    public float maxFlickerDelay = 0.5f; //max between
    public float minIntensity = 1f; //min intensity during flicker
    public float maxIntensity = 3f;

    private float originalIntensity;

    private void Start()
    {
        if (targetLight == null)
        {
            targetLight = GetComponent<Light>();
        }
        originalIntensity = targetLight.intensity;

        // Start flickering
        StartCoroutine(FlickerLight());
    }

    private IEnumerator FlickerLight()
    {
        while (true)
        {
            // Random delay between flickers
            float delay = Random.Range(minFlickerDelay, maxFlickerDelay);
            yield return new WaitForSeconds(delay);

            // Random intensity during flicker
            targetLight.intensity = Random.Range(minIntensity, maxIntensity);

            // Reset to original intensity after a short time
            yield return new WaitForSeconds(0.05f);
            targetLight.intensity = originalIntensity;
        }
    }
}
