namespace Player.Input.Action
{
    public class BasicAttack : IAction
    {
        public void Execute(IPlayerController player)
        {
            if (player.HasStamina(RequiredStamina))
                player.DoAction(this);
        }

        public int RequiredStamina => 30;
    }
}