namespace Player.States
{
    public class BlockState : IPlayerState
    {
        public event BasicEvent OnExit;

        public bool CanMoveTo<T>() where T : IPlayerState
        {
            return typeof(T) == typeof(FreeState);
        }
        
        public bool Is<T>() where T : IPlayerState => typeof(T) == typeof(BlockState);

        public bool CanJump => false;
        public void Exit() => OnExit?.Invoke();
    }
}