public class DoneButton(string letter) : IInteraction
{
    private bool IsActivated = false;
    private readonly InteractionEnum type = InteractionEnum.BUTTON;

    public void SetActivated() {
        IsActivated = !IsActivated;
    }

    public string GetLetter() {
        return letter;
    }

    public bool GetIsActivated() {
        return IsActivated;
    }

    public new InteractionEnum GetType() {
        return type;
    }
}