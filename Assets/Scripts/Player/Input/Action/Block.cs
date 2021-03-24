namespace Player.Input.Action
{
    public class Block : IAction
    {
        public void Execute(IPlayerController player)
        {
            player.DoAction(this);
        }
    }
}