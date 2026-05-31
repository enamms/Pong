using UnityEngine;

/// <summary>
/// Changes the background color whenever the score crosses a defined threshold.
/// Attach this to any GameObject in your scene and assign your background SpriteRenderer.
/// Listens to ScoreManager.Instance each time a point is added.
/// </summary>
public class BackgroundManager : MonoBehaviour
{
    [System.Serializable]
    public class ScoreColorThreshold
    {
        [Tooltip("The score value at which this color activates.")]
        public int scoreThreshold;

        [Tooltip("The background color to switch to at this threshold.")]
        public Color color = Color.white;
    }

    [Header("Background Reference")]
    [Tooltip("The SpriteRenderer of your background GameObject.")]
    public SpriteRenderer backgroundRenderer;

    [Header("Color Thresholds")]
    [Tooltip("Add as many score/color pairs as you want. Order them from lowest to highest threshold.")]
    public ScoreColorThreshold[] thresholds;

    [Header("Transition")]
    [Tooltip("If true, the background smoothly lerps to the target color instead of snapping.")]
    public bool smoothTransition = true;

    [Tooltip("How fast the color lerps to the target (higher = faster).")]
    [Range(0.5f, 20f)]
    public float transitionSpeed = 5f;

    private Color _targetColor;
    private int _lastAppliedThresholdIndex = -1;

    private void Awake()
    {
        if (backgroundRenderer == null)
        {
            Debug.LogWarning("[BackgroundColorController] No SpriteRenderer assigned.");
            return;
        }

        // Initialise target to the current background color
        _targetColor = backgroundRenderer.color;
    }

    private void Update()
    {
        if (backgroundRenderer == null) return;
        if (!smoothTransition) return;

        // Smoothly lerp toward the target color every frame
        backgroundRenderer.color = Color.Lerp(
            backgroundRenderer.color,
            _targetColor,
            transitionSpeed * Time.deltaTime
        );
    }

    /// <summary>
    /// Call this whenever the score changes. ScoreManager calls this automatically
    /// if you wire it up, or you can call it manually from ScoreManager.AddPoint().
    /// </summary>
    public void OnScoreChanged(int newScore)
    {
        if (thresholds == null || thresholds.Length == 0) return;

        // Find the highest threshold that the current score has reached
        int bestMatchIndex = -1;
        for (int i = 0; i < thresholds.Length; i++)
        {
            if (newScore >= thresholds[i].scoreThreshold)
                bestMatchIndex = i;
        }

        // Only trigger a change if we've moved to a new threshold
        if (bestMatchIndex == _lastAppliedThresholdIndex) return;

        _lastAppliedThresholdIndex = bestMatchIndex;

        Color newColor = bestMatchIndex >= 0
            ? thresholds[bestMatchIndex].color
            : backgroundRenderer.color; // No threshold met yet — keep current color

        if (smoothTransition)
        {
            _targetColor = newColor;
        }
        else
        {
            backgroundRenderer.color = newColor;
            _targetColor = newColor;
        }
    }
}