using spaceinvaders.enums;

namespace spaceinvaders.model.screens.SaveScoreScreen;

public class DoneButton(string letter) : IInteraction
{
    private const InteractionEnum Type = InteractionEnum.Button;
    private bool _isActivated;

    public void SetActivated()
    {
        _isActivated = !_isActivated;
    }

    public string GetLetter()
    {
        return letter;
    }

    public bool GetIsActivated()
    {
        return _isActivated;
    }

    public new InteractionEnum GetType()
    {
        return Type;
    }
}