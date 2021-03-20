namespace Player.Input.Action
{
    public class JumpAction : IAction
    {
        public void Execute(IPlayerController player)
        {
            player.Jump();
        }
    }
}