using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Central authority for icon unlocks, equipping, and persistence.
/// Put this on a persistent GameObject (DontDestroyOnLoad) that exists in your
/// first-loaded scene, e.g. a "Managers" object.
/// </summary>
public class IconManager : MonoBehaviour
{
    // public Animator unlockUIAnimator;
    public static IconManager Instance { get; private set; }

    [Tooltip("Every icon in the game, including the default one. Order here defines menu order.")]
    [SerializeField] private List<IconData> allIcons = new List<IconData>();

    private const string UnlockedKey = "IconSystem_UnlockedIds";
    private const string EquippedKey = "IconSystem_EquippedId";

    private HashSet<string> unlockedIds = new HashSet<string>();
    private string equippedId;

    /// <summary>Fired when a new icon is unlocked, e.g. to show a popup. Passes the IconData.</summary>
    public event Action<IconData> OnIconUnlocked;

    /// <summary>Fired whenever the equipped icon changes. Passes the newly equipped IconData.</summary>
    public event Action<IconData> OnIconEquipped;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadProgress();
    }

    // ---------- Public queries ----------

    public IReadOnlyList<IconData> AllIcons => allIcons;

    public bool IsUnlocked(string iconId) => unlockedIds.Contains(iconId);

    public IconData GetEquippedIcon()
    {
        var icon = allIcons.FirstOrDefault(i => i.iconId == equippedId);
        return icon != null ? icon : allIcons.FirstOrDefault();
    }

    // ---------- Unlocking ----------

    /// <summary>
    /// Call this whenever the player's current-run score changes (e.g. every time it increases).
    /// Unlocks any icon whose requiredScore has been met and hasn't been unlocked yet.
    /// </summary>
    public void CheckScoreAchievements(int currentScore)
    {
        foreach (var icon in allIcons)
        {
            if (icon.unlockedByDefault) continue;
            if (unlockedIds.Contains(icon.iconId)) continue;

            if (currentScore >= icon.requiredScore)
            {
                UnlockIcon(icon.iconId);
            }
        }
    }

    /// <summary>Directly unlock an icon by id (e.g. for non-score achievements, IAP, etc).</summary>
    public void UnlockIcon(string iconId)
    {
        if (unlockedIds.Contains(iconId)) return;

        var icon = allIcons.FirstOrDefault(i => i.iconId == iconId);
        if (icon == null)
        {
            Debug.LogWarning($"IconManager: tried to unlock unknown icon id '{iconId}'");
            return;
        }

        unlockedIds.Add(iconId);
        SaveProgress();
        // unlockUIAnimator.SetTrigger("unlock");
        OnIconUnlocked?.Invoke(icon);
    }

    // ---------- Equipping ----------

    public bool TryEquipIcon(string iconId)
    {
        if (!unlockedIds.Contains(iconId))
        {
            Debug.LogWarning($"IconManager: tried to equip locked icon '{iconId}'");
            return false;
        }

        equippedId = iconId;
        SaveProgress();

        var icon = allIcons.FirstOrDefault(i => i.iconId == iconId);
        Debug.Log($"[IconManager] Equipping '{iconId}', found icon: {(icon != null ? icon.displayName : "NULL")}, sprite: {(icon != null && icon.sprite != null ? icon.sprite.name : "NULL")}, listeners on OnIconEquipped: {OnIconEquipped?.GetInvocationList().Length ?? 0}");
        OnIconEquipped?.Invoke(icon);
        return true;
    }

    // ---------- Persistence ----------

    private void LoadProgress()
    {
        unlockedIds.Clear();

        string saved = PlayerPrefs.GetString(UnlockedKey, "");
        if (!string.IsNullOrEmpty(saved))
        {
            foreach (var id in saved.Split(','))
                if (!string.IsNullOrEmpty(id)) unlockedIds.Add(id);
        }

        // Always ensure default-unlocked icons are actually unlocked
        // (covers the case of adding a new default icon after players already have a save).
        foreach (var icon in allIcons)
        {
            if (icon.unlockedByDefault) unlockedIds.Add(icon.iconId);
        }

        equippedId = PlayerPrefs.GetString(EquippedKey, "");
        if (string.IsNullOrEmpty(equippedId) || !unlockedIds.Contains(equippedId))
        {
            // fall back to the first default-unlocked icon
            var fallback = allIcons.FirstOrDefault(i => i.unlockedByDefault);
            equippedId = fallback != null ? fallback.iconId : (allIcons.Count > 0 ? allIcons[0].iconId : "");
        }

        SaveProgress(); // persist any newly-granted defaults
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetString(UnlockedKey, string.Join(",", unlockedIds));
        PlayerPrefs.SetString(EquippedKey, equippedId);
        PlayerPrefs.Save();
    }

    /// <summary>Wipes all unlock/equip progress. Useful for a "reset save" debug button.</summary>
    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey(UnlockedKey);
        PlayerPrefs.DeleteKey(EquippedKey);
        LoadProgress();
    }
}