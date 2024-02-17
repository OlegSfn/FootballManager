namespace UILayer.MenuClasses.MenuButtonsClasses;
using MenuButtonGroupsClasses;

public class RadioButton : MenuButton
{
    private RadioButtonsGroup _radioButtonGroup;
    public bool IsSelected { get; private set; }

    public RadioButton(string text, RadioButtonsGroup radioButtonGroup)
    {
        Text = text; 
        _radioButtonGroup = radioButtonGroup;
    }
    
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