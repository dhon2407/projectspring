namespace Player
{
    public delegate void BasicEvent();
    
    public interface IPlayerController
    {
        event BasicEvent OnGroundLand;
        bool OnGround { get; set; }
        int MoveDirection { get; }
        void Jump();
        void OnJumpInvoke();
        void OnGroundLandInvoke();
        void OnRunStep();
    }
}