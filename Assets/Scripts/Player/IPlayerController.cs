﻿using Player.Input.Action;
using Player.States;

namespace Player
{
    public delegate void BasicEvent();
    public delegate void ChangeStateEvent(IPlayerState state);
    
    public interface IPlayerController
    {
        event BasicEvent OnGroundLand;
        bool OnGround { get; set; }
        IPlayerState CurrentState { get; set; }
        int MoveDirection { get; }
        void Jump();
        void OnJumpInvoke();
        void OnGroundLandInvoke();
        void OnRunStep();
        void DoAction(IAction action);
    }
}