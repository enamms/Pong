using UnityEngine;
using UnityEngine.SceneManagement; // Required for changing scenes
using System.Collections;

public class GameOver : MonoBehaviour
{
    // Call this when the "Play Again" button is pressed
    public void PlayAgain()
    {
        StartCoroutine(PlayAgainWithDelay());
    }

    private IEnumerator PlayAgainWithDelay()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Call this when the "Menu" button is pressed
    public void GoToMainMenu()
    {
        StartCoroutine(GoToMainMenuWithDelay());
    }

    private IEnumerator GoToMainMenuWithDelay()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("MainMenu");
    }
}