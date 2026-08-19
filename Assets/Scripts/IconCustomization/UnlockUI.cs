using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UnlockUI : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // Subscribe to the event when this UI object comes alive
        if (IconManager.Instance != null)
        {
            IconManager.Instance.OnIconUnlocked += HandleIconUnlocked;
        }
    }

    private void OnDisable()
    {
        // Always unsubscribe when disabled or destroyed to avoid memory leaks
        if (IconManager.Instance != null)
        {
            IconManager.Instance.OnIconUnlocked -= HandleIconUnlocked;
        }
    }

    private void HandleIconUnlocked(IconData unlockedIcon)
    {
        // Trigger your UI animation
        if (animator != null)
        {
            animator.SetTrigger("unlock");
        }
    }
}