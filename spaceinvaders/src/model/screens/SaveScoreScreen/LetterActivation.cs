public class LetterActivation(string letter) : IInteraction
{
    private bool IsActivated = false;
    private readonly InteractionEnum type = InteractionEnum.TEXT;

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