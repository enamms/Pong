using UnityEngine;

/// <summary>
/// DashedCircleController
/// Attach to your dashed circle GameObject.
/// Handles player rotation input and exposes spin velocity for the ball to read on impact.
/// </summary>
public class CircleController : MonoBehaviour
{
    // ── Inspector Settings ────────────────────────────────────────────────────

    [Header("Rotation Settings")]
    [Tooltip("How fast the circle rotates in response to drag input.")]
    public float rotationSensitivity = 0.4f;

    [Tooltip("Enable momentum so the circle keeps spinning after the player lifts their finger.")]
    public bool enableMomentum = true;

    [Tooltip("How quickly momentum fades (higher = stops faster).")]
    [Range(1f, 20f)]
    public float momentumDamping = 8f;

    [Header("Dash Segments")]
    [Tooltip("Number of dashes on the circle. Must match your sprite/visual exactly.")]
    public int dashCount = 8;

    [Tooltip("Each dash covers this many degrees of the circle (the rest is a gap).")]
    [Range(5f, 40f)]
    public float dashAngleDegrees = 25f;

    [Header("Circle Geometry")]
    [Tooltip("The circle's radius in world units. Must match your sprite size.")]
    public float circleRadius = 2.5f;

    // ── Public Read-Only State (used by BallController) ───────────────────────

    /// <summary>Current spin speed in degrees/sec. Positive = counter-clockwise.</summary>
    public float SpinVelocity { get; private set; }

    // ── Internal State ────────────────────────────────────────────────────────

    private Vector2 _previousInputPosition;
    private bool    _isDragging = false;
    private Camera  _mainCamera;

    // ─────────────────────────────────────────────────────────────────────────

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleInput();

        if (!_isDragging && enableMomentum)
            ApplyMomentum();

        if (!_isDragging)
            SpinVelocity = Mathf.Lerp(SpinVelocity, 0f, momentumDamping * Time.deltaTime);
    }

    // ── Input ─────────────────────────────────────────────────────────────────

    private void HandleInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 0)
        {
            _isDragging = false;
            return;
        }

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                _previousInputPosition = touch.position;
                _isDragging            = true;
                SpinVelocity           = 0f;
                break;

            case TouchPhase.Moved:
                if (_isDragging)
                    RotateFromDelta(touch.position, touch.deltaTime);
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                _isDragging = false;
                break;
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _previousInputPosition = Input.mousePosition;
            _isDragging            = true;
            SpinVelocity           = 0f;
        }
        else if (Input.GetMouseButton(0) && _isDragging)
        {
            RotateFromDelta(Input.mousePosition, Time.deltaTime);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }
    }

    // ── Rotation ──────────────────────────────────────────────────────────────

    private void RotateFromDelta(Vector2 currentInputPosition, float deltaTime)
    {
        Vector2 circleScreenPos = _mainCamera.WorldToScreenPoint(transform.position);

        Vector2 fromVec = _previousInputPosition - circleScreenPos;
        Vector2 toVec   = currentInputPosition   - circleScreenPos;

        float angle = Vector2.SignedAngle(fromVec, toVec) * rotationSensitivity;

        transform.Rotate(0f, 0f, angle);

        if (deltaTime > 0f)
            SpinVelocity = angle / deltaTime;

        _previousInputPosition = currentInputPosition;
    }

    private void ApplyMomentum()
    {
        if (Mathf.Approximately(SpinVelocity, 0f)) return;

        transform.Rotate(0f, 0f, SpinVelocity * Time.deltaTime);
        SpinVelocity = Mathf.Lerp(SpinVelocity, 0f, momentumDamping * Time.deltaTime);

        if (Mathf.Abs(SpinVelocity) < 0.01f)
            SpinVelocity = 0f;
    }

    // ── Segment Detection ─────────────────────────────────────────────────────

    /// <summary>
    /// Given a world-space contact point, returns true if it lands on a dash, false if on a gap.
    /// </summary>
    public bool IsPointOnDash(Vector2 worldPoint)
    {
        Vector2 localDir   = worldPoint - (Vector2)transform.position;
        float   worldAngle = Mathf.Atan2(localDir.y, localDir.x) * Mathf.Rad2Deg;

        // Convert to local space by removing the circle's own rotation
        float localAngle = worldAngle - transform.eulerAngles.z;

        // Normalise to [0, 360)
        localAngle = (localAngle % 360f + 360f) % 360f;

        float segmentAngle = 360f / dashCount;
        float posInSegment = localAngle % segmentAngle;

        return posInSegment <= dashAngleDegrees;
    }

    // ── Public Helpers ────────────────────────────────────────────────────────

    public void StopMomentum()
    {
        SpinVelocity = 0f;
        _isDragging  = false;
    }

    public void SetRotation(float angleDegrees)
    {
        transform.rotation = Quaternion.Euler(0f, 0f, angleDegrees);
        SpinVelocity       = 0f;
    }
}