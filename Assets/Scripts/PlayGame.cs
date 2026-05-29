using UnityEngine;
using UnityEngine.SceneManagement; // Required for changing scenes

public class PlayGame : MonoBehaviour
{
    // Call this function when the Play button is clicked
    public void Play()
    {
        // // Loads the next scene in the Build Settings queue
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
        // ALTERNATIVE: You can also load by the exact scene name if you prefer:
        SceneManager.LoadScene("Game"); 
    }
}