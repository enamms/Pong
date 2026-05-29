using UnityEngine;
using UnityEngine.SceneManagement; // Required for changing scenes

public class GameOver : MonoBehaviour
{
    // Call this when the "Play Again" button is pressed
    public void PlayAgain()
    {
        // Reloads the currently active scene, which resets the game and the score
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Call this when the "Menu" button is pressed
    public void GoToMainMenu()
    {
        // Loads the Main Menu scene. 
        // Replace "MainMenuScene" with the exact name of your main menu scene file.
        SceneManager.LoadScene("MainMenu"); 
    }
}