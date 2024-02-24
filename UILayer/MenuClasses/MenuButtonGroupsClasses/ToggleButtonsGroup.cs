using UILayer.MenuClasses.MenuButtonsClasses;

namespace UILayer.MenuClasses.MenuButtonGroupsClasses;

/// <summary>
/// Represents a group of toggle buttons.
/// </summary>
public class ToggleButtonsGroup : ButtonsGroup
{
    public HashSet<ToggleButton> SelectedToggleButtons { get; } = new();

    /// <summary>
    /// Updates the set of toggled buttons based on the specified toggle button.
    /// </summary>
    /// <param name="toggleButton">The toggle button to update.</param>
    public void UpdateToggledButtons(ToggleButton toggleButton)
    {
        if (!SelectedToggleButtons.Add(toggleButton))
            SelectedToggleButtons.Remove(toggleButton);
    }
}