using UnityEngine;

public class mainmenuball : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Launch the object in a random diagonal direction
        Launch();
    }

    void Launch()
    {
        // Pick a random direction avoiding pure horizontal/vertical axes
        float x = Random.Range(0, 2) == 0 ? -1f : 1f;
        float y = Random.Range(0, 2) == 0 ? -1f : 1f;

        Vector2 randomDirection = new Vector2(x, y).normalized;
        rb.linearVelocity = randomDirection * speed; // Use 'velocity' if on Unity 2022 or older
    }

    void FixedUpdate()
    {
        // Ensure speed remains constant (prevents floating point drag or accidental slowdowns)
        if (rb.linearVelocity != Vector2.zero)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * speed;
        }
    }
}