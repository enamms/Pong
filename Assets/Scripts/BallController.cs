using UnityEngine;

/// <summary>
/// Simplified BallController using Unity's built-in 2D Physics.
/// Requires a Rigidbody2D and a CircleCollider2D on this GameObject.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class BallController : MonoBehaviour
{
    [Header("Speed Settings")]
    [Tooltip("Starting speed of the ball.")]
    public float initialSpeed = 4f;

    [Tooltip("Maximum speed the ball can reach.")]
    public float maxSpeed = 12f;

    [Tooltip("How much speed is added per bounce.")]
    public float speedIncreasePerBounce = 0.3f;

    [Header("Scoring")]
    [Tooltip("Assign the Layer that your circle boundary GameObject sits on.")]
    public LayerMask circleBoundaryLayer;

    [Header("Sound Effects")]
    [Tooltip("First pop sound clip.")]
    public AudioClip popSound1;

    [Tooltip("Second pop sound clip.")]
    public AudioClip popSound2;

    [Tooltip("Volume of the pop sounds.")]
    [Range(0f, 1f)]
    public float popVolume = 1f;

    private Rigidbody2D _rb;
    private float _currentSpeed;
    private bool _isAlive = false;

    private AudioSource _audioSource;
    private bool _useFirstSound = true;  // Alternating tracker

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        // Ensure gravity doesn't pull the ball down
        _rb.gravityScale = 0f;
        // Continuous collision prevents fast-moving balls from slipping through colliders
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        // Add an AudioSource component at runtime if one isn't already attached
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            _audioSource = gameObject.AddComponent<AudioSource>();

        _audioSource.playOnAwake = false;
    }

    private void Start()
    {
        LaunchBall();
    }

    /// <summary>
    /// Spawns the ball at the center and shoots it in a random direction.
    /// </summary>
    public void LaunchBall()
    {
        transform.position = Vector2.zero;
        _currentSpeed = initialSpeed;

        float randomAngle = Random.Range(0f, 360f);
        Vector2 direction = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));

        _rb.linearVelocity = direction * _currentSpeed;
        _isAlive = true;
    }

    /// <summary>
    /// Automatically called by Unity when the ball hits any 2D Collider.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isAlive) return;

        // ── Score + SFX: only when touching the circle boundary layer ─────────
        if (((1 << collision.gameObject.layer) & circleBoundaryLayer) != 0)
        {
            ScoreManager.Instance?.AddPoint();
            PlayAlternatingPop();
        }

        // ── Physics: speed bump on every bounce ───────────────────────────────
        Vector2 reflectDirection = _rb.linearVelocity.normalized;

        _currentSpeed += speedIncreasePerBounce;
        _currentSpeed = Mathf.Clamp(_currentSpeed, initialSpeed, maxSpeed);

        _rb.linearVelocity = reflectDirection * _currentSpeed;
    }

    /// <summary>
    /// Plays pop1 and pop2 in alternation each time the ball hits the circle.
    /// </summary>
    private void PlayAlternatingPop()
    {
        AudioClip clipToPlay = _useFirstSound ? popSound1 : popSound2;

        if (clipToPlay != null)
            _audioSource.PlayOneShot(clipToPlay, popVolume);

        _useFirstSound = !_useFirstSound;
    }

    public void FreezeBall()
    {
        _isAlive = false;
        _rb.linearVelocity = Vector2.zero;
    }
}