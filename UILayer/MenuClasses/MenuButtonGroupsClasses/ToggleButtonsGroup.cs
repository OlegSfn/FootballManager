using UILayer.MenuClasses.MenuButtonsClasses;

namespace UILayer.MenuClasses.MenuButtonGroupsClasses;

public class ToggleButtonsGroup
{
    public HashSet<ToggleButton> SelectedToggleButtons { get; } = new();

    public void UpdateToggledButtons(ToggleButton toggleButton)
    {
        if (SelectedToggleButtons.Contains(toggleButton))
            SelectedToggleButtons.Remove(toggleButton);
        else
            SelectedToggleButtons.Add(toggleButton);
    }
}