public interface IInteraction {
    public void SetActivated();
    public string GetLetter();
    public bool GetIsActivated();
    public InteractionEnum GetType();
}