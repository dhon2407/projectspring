namespace Player.Input.Action
{
    public class JumpAction : IAction
    {
        public void Execute(IPlayerController player)
        {
            if (player.HasStamina(RequiredStamina))
            {
                player.CurrentAction = this;
                player.Jump();
            }
        }

        public int RequiredStamina => 35;
    }
}