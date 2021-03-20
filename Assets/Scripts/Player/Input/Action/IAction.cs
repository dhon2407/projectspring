namespace Player.Input.Action
{
    public interface IAction
    {
        void Execute(IPlayerController player);
    }
}