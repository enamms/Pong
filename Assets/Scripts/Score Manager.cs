using UnityEngine;
using TMPro;

/// <summary>
/// Manages the current score and all-time high score.
/// Attach this to a GameObject in your game scene.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("UI References")]
    [Tooltip("TextMeshPro label that displays the current score.")]
    public TextMeshProUGUI scoreText;

    [Tooltip("TextMeshPro label that displays the all-time high score.")]
    public TextMeshProUGUI highScoreText;

    private const string HIGH_SCORE_KEY = "HighScore";

    private int _currentScore = 0;
    private int _highScore = 0;

    private void Awake()
    {
        // Simple Singleton setup for the scene. 
        // We removed DontDestroyOnLoad so a fresh manager takes over when the scene reloads.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Load the saved high score on startup
        _highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    }

    private void Start()
    {
        // Ensure score starts at 0 on scene load/reload
        _currentScore = 0; 
        RefreshUI();
    }

    /// <summary>
    /// Call this every time the ball touches the circle boundary.
    /// </summary>
    public void AddPoint()
    {
        _currentScore++;

        if (_currentScore > _highScore)
        {
            _highScore = _currentScore;
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, _highScore);
            PlayerPrefs.Save(); // Flush to disk immediately
        }

        RefreshUI();
    }

    /// <summary>
    /// Resets the current score to zero (e.g. on game over / restart).
    /// </summary>
    public void ResetScore()
    {
        _currentScore = 0;
        RefreshUI();
    }

    /// <summary>
    /// Wipes the saved high score from disk and resets both values.
    /// </summary>
    public void ClearHighScore()
    {
        _highScore = 0;
        _currentScore = 0;
        PlayerPrefs.DeleteKey(HIGH_SCORE_KEY);
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {_currentScore}";

        if (highScoreText != null)
            highScoreText.text = $"Best: {_highScore}";
    }

    // ── Optional read-only accessors ──────────────────────────────────────────
    public int CurrentScore => _currentScore;
    public int HighScore    => _highScore;
}