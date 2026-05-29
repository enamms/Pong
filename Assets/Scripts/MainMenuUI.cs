using UnityEngine;
using TMPro;

/// <summary>
/// Reads the all-time high score and displays it on the Main Menu.
/// Attach this to any GameObject in your Main Menu scene.
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("TextMeshPro label that will show the all-time high score.")]
    public TextMeshProUGUI highScoreText;

    [Header("Display Settings")]
    [Tooltip("Text shown when the player has not scored yet.")]
    public string noScoreMessage = "No score yet — play your first game!";

    private void Start()
    {
        DisplayHighScore();
    }

    private void DisplayHighScore()
    {
        if (highScoreText == null)
        {
            Debug.LogWarning("MainMenuUI: highScoreText is not assigned in the Inspector.");
            return;
        }

        // PlayerPrefs is available immediately — no dependency on ScoreManager being loaded
        int savedHighScore = PlayerPrefs.GetInt("HighScore", 0);

        highScoreText.text = savedHighScore > 0
            ? $"Best: {savedHighScore}"
            : noScoreMessage;
    }
}