using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* This script is used to display the game over screen. 
Used when the player runs our of health, in the 
PlayerCharacter.cs script
*/
public class GameManager : MonoBehaviour
{

    public GameObject gameOverUI;
    public GameObject victoryUI;
     public GameObject weaponSwitching;
     public FPS_Input fpsInput;          

    public void gameOver(){
        // Unhide the game over screen in canvas
        gameOverUI.SetActive(true);     

        // Dsiable weapons
        weaponSwitching.SetActive(false);    

         // Make the mouse usable in game over screen        
        Cursor.lockState = CursorLockMode.None;     
        Cursor.visible = true;                     
    }

    public void Victory()
    {
        // Unhide the game over screen in canvas
        victoryUI.SetActive(true);     

        // Dsiable weapons
        weaponSwitching.SetActive(false);    

         fpsInput.SetMovement(false);            // Stop player movement on victory       

         // Make the mouse usable in game over screen        
        Cursor.lockState = CursorLockMode.None;     
        Cursor.visible = true;  
        
    }

    public void Restart(){
        // If restart button click, restart game scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
