using System;
using System.Collections.Generic;
using Player.Input.Action;

namespace Player.Input
{
    public interface IInputHandler
    {
        int MovementDirection { get; }
        IAction CheckInputAction { get; }
        IAction ConsumeCurrentInputAction { get; }
        void TemporaryStopMovement(float? duration = null);
        void Suspend(bool includingMovement, float? duration = null);
        void CancelSuspend(bool includingMovement);
        void ResumeMovement();
        void BlockActions(Type actionType);
        void UnblockActions(Type actionType);

    }
}