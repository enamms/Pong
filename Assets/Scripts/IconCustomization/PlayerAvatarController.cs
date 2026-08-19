using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Put this on your player/ball GameObject alongside its Image component
/// (use this version if your ball is a UI element under a Canvas, as opposed
/// to a world-space object with a SpriteRenderer).
/// Automatically shows whichever icon is currently equipped, and updates
/// live if the player changes their icon in the customize menu.
/// </summary>
[RequireComponent(typeof(Image))]
public class PlayerAvatarController : MonoBehaviour
{
    private Image iconImage;

    private void Awake()
    {
        iconImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        ApplyEquippedIcon();

        if (IconManager.Instance != null)
            IconManager.Instance.OnIconEquipped += HandleIconEquipped;
    }

    private void OnDisable()
    {
        if (IconManager.Instance != null)
            IconManager.Instance.OnIconEquipped -= HandleIconEquipped;
    }

    private void ApplyEquippedIcon()
    {
        if (IconManager.Instance == null)
        {
            Debug.Log("[PlayerAvatarController] IconManager.Instance is NULL");
            return;
        }

        var icon = IconManager.Instance.GetEquippedIcon();
        Debug.Log($"[PlayerAvatarController] Applying equipped icon: {(icon != null ? icon.displayName : "NULL")}, sprite: {(icon != null && icon.sprite != null ? icon.sprite.name : "NULL")}");
        if (icon != null && icon.sprite != null)
            iconImage.sprite = icon.sprite;
    }

    private void HandleIconEquipped(IconData icon)
    {
        Debug.Log($"[PlayerAvatarController] HandleIconEquipped event received: {(icon != null ? icon.displayName : "NULL")}");
        if (icon != null && icon.sprite != null)
            iconImage.sprite = icon.sprite;
    }
}