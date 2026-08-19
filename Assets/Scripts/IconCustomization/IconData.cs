using UnityEngine;

/// <summary>
/// Defines a single player avatar icon: its visuals and how it's unlocked.
/// Create instances via Assets > Create > Icons > Icon Data.
/// </summary>
[CreateAssetMenu(fileName = "NewIcon", menuName = "Icons/Icon Data")]
public class IconData : ScriptableObject
{
    [Tooltip("Unique identifier used for saving/loading. Never change this after players have unlocked it.")]
    public string iconId;

    [Tooltip("Name shown to the player in the customize menu.")]
    public string displayName = "Icon";

    [Tooltip("Sprite used both in-game (as the player avatar) and in the menu grid.")]
    public Sprite sprite;

    [Tooltip("If true, this icon is unlocked from the very start (e.g. the default ball).")]
    public bool unlockedByDefault = false;

    [Tooltip("Score the player must reach in a single run to unlock this icon. Ignored if unlockedByDefault is true.")]
    public int requiredScore = 0;

    [Tooltip("Optional flavor text shown under the icon when locked, e.g. 'Reach a score of 20'.")]
    public string GetUnlockHint()
    {
        if (unlockedByDefault) return "Unlocked";
        return $"Reach a score of {requiredScore}";
    }
}