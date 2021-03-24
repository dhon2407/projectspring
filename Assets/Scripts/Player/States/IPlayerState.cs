namespace Player.States
{
    public delegate void BasicEvent();
    
    public interface IPlayerState
    {
        event BasicEvent OnExit;
        bool CanMoveTo<T>() where T : IPlayerState;
        bool CanJump { get; }
        void Exit();
        bool Is<T>() where T : IPlayerState;
    }
}