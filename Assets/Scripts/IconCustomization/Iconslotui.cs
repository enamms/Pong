using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// One clickable slot in the customize menu grid. Attach to a prefab with:
/// - Button (root or child)
/// - Image "iconImage" for the sprite
/// - GameObject "lockOverlay" (a dark panel + lock icon) shown when locked
/// - GameObject "selectedHighlight" shown when this is the equipped icon
/// - Text/TMP_Text "hintText" (optional) for the unlock requirement
/// </summary>
public class IconSlotUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Button button;
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject lockOverlay;
    [SerializeField] private GameObject selectedHighlight;
    [SerializeField] private Text hintText; // swap for TMP_Text if you use TextMeshPro

    private IconData data;
    private CustomizeMenuUI parentMenu;

    public void Setup(IconData iconData, CustomizeMenuUI menu)
    {
        data = iconData;
        parentMenu = menu;

        iconImage.sprite = data.sprite;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(HandleClicked);

        Refresh();
    }

    /// <summary>Re-reads unlock/equip state from IconManager and updates visuals.</summary>
    public void Refresh()
    {
        bool unlocked = IconManager.Instance.IsUnlocked(data.iconId);
        bool equipped = IconManager.Instance.GetEquippedIcon() == data;

        if (lockOverlay != null) lockOverlay.SetActive(!unlocked);
        if (selectedHighlight != null) selectedHighlight.SetActive(equipped);

        // Dim the icon a bit while locked so the shape is still visible (Geometry Dash style)
        iconImage.color = unlocked ? Color.white : new Color(1f, 1f, 1f, 0.35f);

        if (hintText != null)
            hintText.text = unlocked ? data.displayName : data.GetUnlockHint();
    }

    private void HandleClicked()
    {
        if (!IconManager.Instance.IsUnlocked(data.iconId))
        {
            // Optional: play a "locked" sound/shake here instead of doing nothing
            return;
        }

        IconManager.Instance.TryEquipIcon(data.iconId);
        parentMenu.RefreshAllSlots();
    }
}