namespace Player.States
{
    public class AttackState : IPlayerState
    {
        public event BasicEvent OnExit;

        public bool CanMoveTo<T>() where T : IPlayerState
        {
            return true;
        }
        
        public bool Is<T>() where T : IPlayerState => typeof(T) == typeof(AttackState);

        public bool CanJump => false;
        public void Exit() => OnExit?.Invoke();
        
    }
}