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

    private Rigidbody2D _rb;
    private float _currentSpeed;
    private bool _isAlive = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        // Ensure gravity doesn't pull the ball down
        _rb.gravityScale = 0f;
        // Continuous collision prevents fast-moving balls from slipping through colliders
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
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
        transform.position = Vector2.zero; // Or set to your circle's center position
        _currentSpeed = initialSpeed;

        // Pick a completely random direction
        float randomAngle = Random.Range(0f, 360f);
        Vector2 direction = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));

        // Apply velocity to the Rigidbody
        _rb.linearVelocity = direction * _currentSpeed;
        _isAlive = true;
    }

    /// <summary>
    /// Automatically called by Unity when the ball hits any 2D Collider.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isAlive) return;

        // 1. Get the direction the ball is already bouncing toward via physics reflection
        Vector2 reflectDirection = _rb.linearVelocity.normalized;

        // 2. Safely bump the speed up slightly
        _currentSpeed += speedIncreasePerBounce;
        _currentSpeed = Mathf.Clamp(_currentSpeed, initialSpeed, maxSpeed);

        // 3. Re-assign the velocity with the new speed
        _rb.linearVelocity = reflectDirection * _currentSpeed;
    }

    public void FreezeBall()
    {
        _isAlive = false;
        _rb.linearVelocity = Vector2.zero;
    }
}