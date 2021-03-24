namespace Player.States
{
    public class FreeState : IPlayerState
    {
        public event BasicEvent OnExit;
        public bool CanMoveTo<T>() where T : IPlayerState => true;
        public bool Is<T>() where T : IPlayerState => typeof(T) == typeof(FreeState);
        public bool CanJump => true;
        public void Exit() => OnExit?.Invoke();
    }
}