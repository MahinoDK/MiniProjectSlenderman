using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TMPro.Examples;
using UnityEngine.UI; 

public class MessageDisplay : MonoBehaviour
{
    public GameObject StartMessage;
    public float displayDuration = 3f; //vis i 3 sekunder thx

    public GameObject blackScreenUI;
    public TextMeshProUGUI pageCountText;
    public float fadeDuration = 1.5f;

    private GameLogic gameLogic; //reference other script
    private bool hasDisplayedEndUI = false;

    void Start()
    {
        StartCoroutine(DisplayMessage());

        gameLogic = FindObjectOfType<GameLogic>();
        if (gameLogic == null)
        {
            Debug.LogError("Cannot find gamelogic in messagescript");
        }

        //Insure inital hidden ui but also make fully transparent to make it fade in
        if(blackScreenUI != null)
        {
            Image blackScreenImage = blackScreenUI.GetComponent<Image>();
            if (blackScreenImage != null)
            {
                blackScreenImage.color = new Color(0, 0, 0, 0); // Fully transparent
            }

            blackScreenUI.SetActive(false);
        }

        if (pageCountText != null)
        {
            pageCountText.color = new Color(pageCountText.color.r, pageCountText.color.g, pageCountText.color.b, 0); // Fully transparent
        }
    }

    void Update()
    {
        // Check if the player is dead and the end UI hasn't been displayed yet
        if (gameLogic != null && gameLogic.isDead && !hasDisplayedEndUI)
        {
            StartCoroutine(DisplayEndUI());
            hasDisplayedEndUI = true;
        }
    }

    // Update is called once per frame
    private IEnumerator DisplayMessage()
    {
        StartMessage.SetActive(true);

        yield return new WaitForSeconds(displayDuration);

        // Gradually reduce the alpha
        TextMeshProUGUI textComponent = StartMessage.GetComponent<TextMeshProUGUI>();
        Color originalColor = textComponent.color;

        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - t);
            yield return null;
        }

        textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        StartMessage.SetActive(false);
    }
    private IEnumerator DisplayEndUI()
    {
        // Wait for 2 seconds after the player dies
        yield return new WaitForSeconds(4f);

        if (blackScreenUI != null)
        {
            blackScreenUI.SetActive(true);

            Image blackScreenImage = blackScreenUI.GetComponent<Image>();
            TextMeshProUGUI textComponent = pageCountText;

            float elapsedTime = 0;

            // Gradually fade in both the black screen and the text
            while (elapsedTime < fadeDuration)
            {
                float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);

                if (blackScreenImage != null)
                {
                    blackScreenImage.color = new Color(0, 0, 0, alpha);
                }

                if (textComponent != null)
                {
                    textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, alpha);
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure full visibility after the fade-in
            if (blackScreenImage != null)
            {
                blackScreenImage.color = new Color(0, 0, 0, 1);
            }

            if (textComponent != null)
            {
                textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 1);
                textComponent.text = "Pages Collected: " + gameLogic.pageCount + "/8";
            }
        }
    }
}