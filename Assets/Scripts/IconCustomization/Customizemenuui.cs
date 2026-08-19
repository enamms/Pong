using UnityEngine;

/// <summary>
/// Controls the customize screen: builds a grid of IconSlotUI from IconManager's
/// icon list. Attach to the panel/screen GameObject that holds the grid.
/// </summary>
public class CustomizeMenuUI : MonoBehaviour
{
    [Tooltip("Prefab with an IconSlotUI component on it.")]
    [SerializeField] private IconSlotUI slotPrefab;

    [Tooltip("Parent transform with a GridLayoutGroup (or similar) that slots get spawned into.")]
    [SerializeField] private Transform gridParent;

    private readonly System.Collections.Generic.List<IconSlotUI> spawnedSlots
        = new System.Collections.Generic.List<IconSlotUI>();

    private bool built = false;

    /// <summary>Call this from your "Customize" button in the main menu, e.g. via OnClick.</summary>
    public void Open()
    {
        gameObject.SetActive(true);

        if (!built)
        {
            BuildGrid();
            built = true;
        }
        else
        {
            RefreshAllSlots();
        }
    }

    /// <summary>Call this from a "Back"/"Close" button on the screen.</summary>
    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void BuildGrid()
    {
        foreach (var icon in IconManager.Instance.AllIcons)
        {
            var slot = Instantiate(slotPrefab, gridParent);
            slot.Setup(icon, this);
            spawnedSlots.Add(slot);
        }
    }

    public void RefreshAllSlots()
    {
        foreach (var slot in spawnedSlots)
            slot.Refresh();
    }
}