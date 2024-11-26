using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPage : MonoBehaviour
{
    private GameObject page; //reference the gameobject this script is attached to, the pages
    private bool inReach; //The long cube indicating if we are in reach or not

    private GameObject gameLogic; //tilføjer gamelogic object

    private void Start()
    {
        inReach = false; //look up

        gameLogic = GameObject.FindWithTag("GameLogic"); //Identifiser vores gamelogic object så vi kan call herinde

        page = this.gameObject; //we identify that this gameObject is page
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach") //if reachtool the long box touches this gameobject theh
        {
            inReach = true; //true and
            PickUpText.instance.EnableText(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach") //the opposite happen when not in reach or exiting reach
        {
            inReach = false;
            PickUpText.instance.EnableText(false);
        }
    }

    private void Update()
    {
        if(inReach && Input.GetButtonDown("pickup")) //if we are in reach and press pickup button we set up in player preferences
        {
            gameLogic.GetComponent<GameLogic>().pageCount += 1; //Grabbing component of gamelogic the script and want pageCount
            PickUpText.instance.PlaySound();
            PickUpText.instance.EnableText(false);
            page.SetActive(false); //the page turns off it is no longer in sceen we took it
            inReach = false; //no longer in reach of the object we have it
        }
    }
}
