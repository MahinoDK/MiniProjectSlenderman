# MiniProjectSlenderman
Mini Project
I wanted to make a game like slenderman because i loved it growing up, and later realised it was made by unity. I thought it doable and wanted to try and make it myself. I then made a to do list to make it in unity, and updated it along the way.

PlayerMovement Script
I have a PlayerMovement script that handles with Unity CharacterController, Controls the players camera rotation with mouse with a restriction to not look straight up or straight down. A Zoom function with fire2 adjusting Field of view and playing a sound effect. Controls walking/running sound, running just sped up the same sound.

CollectPage Script
This script references the GameLogic script. To interact and pickup the pages, with the reach game object an invisible long box collider attached to player, it checks if bool inReach, if the reach object is collided with the pages gameObject, then a ui will become visible displaying a singleton PickUpText(script) “Pick Up (E)” and the page will be destroyed upon interaction and pageCount will go up with GameLogic.

PickUpText Script
A script made to make PickUpText a singleton to be called in other scripts. This script kept on a gameobject to hold ui text and pageCollectSound, when in reach in the CollectPageScript we call the singleton here and SetActiveState(True). The Page Prefabs has the CollectPage Script on it, and cannot hold the sound or text ui, and therefore this script was made to hold it and easily be called in the CollectPage Script.

GameLogic Script
It keeps track of the total number of pages collected and updates the counter ui object in real time visually for the player. It holds the prefabs for the 8 pages and 15 possible spawnPoint locations, it is randomly chosen between them. It has HandlePlayerDeath where it interacts with the PlayerMovement script to disable all movement during death. It Interacts with the Slenderman script to smoothly rotate towards him upon death. When the player dies, it disables all movement, then rotates towards slenderman, triggers a VFX and disables all sound. It also has a public isDead bool to keep track of is dead for other scripts to activate something upon player death.

MessageDisplay Script
This script displays the start message and smoothly removes it again, fading it out upon starting the game. It also displays the end message upon player death, triggered when isDead = true, showing total pages collected when dead, referencing to GameLogic.



Slenderman Script
This script handles the enemy AI, making sure it is always facing the player's direction; it constantly checks where the player is and teleports near it. It has an AdjustAggression function that updates when the pageCount goes up. Increasing the drain rate of players health, increasing the chase probability and decreasing teleport range to player. This script holds the player's health, and the static effect. If the player is looking at slenderman in 45 angle, and is in specific range, the static will be activated and health will be drained. 

SoundController Script
This script is used on 3 different game objects with the name Cricket, Cricket1 and Cricket 2. It holds three different cricket audio sources, and the script makes sure that only one of them is played at a time, and the sounds are played in random order for a random amount of time, where it has a min and max interval time of how long it can be before a sound should start.

MusicController Script
This controller has 4 music clips, and is made to communicate with the gameLogic where  when the first page is collected the first scary music will be played, then when the third page is collected, a scarier music, fifths scarier and 7th the most intense scary music.

LightFlicker Script
A script that controls intensity of light and flicker delays not turning it all the way off but dimming the lights in a scary random order.

FlashLight Script
Turning it on and off with F button with SetActive true or false, playing a sound as well, a tiltAmount added to tilt downwards when running to make it seem more scary you can not look as well.





