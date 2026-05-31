using UnityEngine;
using UnityEngine.SceneManagement; // Required for changing scenes
using System.Collections; // Required for Coroutines (IEnumerator)

public class PlayGame : MonoBehaviour
{
    [SerializeField] private float delayInSeconds = 1.0f; // Adjust the delay time in the Inspector

    // Call this function when the Play button is clicked
    public void Play()
    {
        // Start the coroutine to handle the delay
        StartCoroutine(LoadSceneWithDelay());
    }

    // This is the coroutine that handles the timer
    private IEnumerator LoadSceneWithDelay()
    {
        // Optional: Trigger a button click sound or animation here

        // Wait for the specified number of seconds
        yield return new WaitForSeconds(delayInSeconds);

        // Load the next scene
        SceneManager.LoadScene("Game");
    }
}