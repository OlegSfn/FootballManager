namespace UILayer.MenuClasses.MenuButtonsClasses;
using MenuButtonGroupsClasses;

/// <summary>
/// Represents a button in a menu that behaves like a radio button, allowing selection within a group.
/// </summary>
public class RadioButton : MenuButton
{
    private RadioButtonsGroup _radioButtonGroup;
    public bool IsSelected { get; private set; }

    public RadioButton(string text, RadioButtonsGroup radioButtonGroup)
    {
        Text = text; 
        _radioButtonGroup = radioButtonGroup;
    }
    
    /// <summary>
    /// Selects this button in her RadioButtonsGroup.
    /// </summary>
    /// <param name="key">The console key to register the action for.</param>
    public override void RegisterAction(ConsoleKey key)
    {
        if (key == ConsoleKey.Spacebar)
        {
            if (_radioButtonGroup.SelectedButton != null) 
                _radioButtonGroup.SelectedButton.IsSelected = false;
            
            _radioButtonGroup.SelectedButton = this;
            IsSelected = true;
        }
    }

    public override string ToString()
    {
        return $"{(IsSelected ? '●' : '○')} {Text}";
    }
}