using Player.Input.Action;
using Player.States;

namespace Player
{
    public delegate void BasicEvent();
    public delegate void ChangeStateEvent(IPlayerState state);
    
    public interface IPlayerController
    {
        event BasicEvent OnGroundLand;
        bool OnGround { get; set; }
        bool IsDead { get; }
        IPlayerState CurrentState { get; set; }
        IAction CurrentAction { get; set; }
        int MoveDirection { get; }
        void Jump();
        void OnJumpInvoke();
        void OnGroundLandInvoke();
        void OnRunStep();
        void DoAction(IAction action);
        bool HasStamina(int requiredStamina);
    }
}