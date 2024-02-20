public class LetterActivation(char letter)
{
    private bool IsActivated = false;

    public void SetActivated() {
        IsActivated = !IsActivated;
    }

    public char GetLetter() {
        return letter;
    }

    public bool GetIsActivated() {
        return IsActivated;
    }
}