namespace Player.Input.Action
{
    public class BasicAttack : IAction
    {
        public void Execute(IPlayerController player)
        {
            player.DoAction(this);
        }
    }
}