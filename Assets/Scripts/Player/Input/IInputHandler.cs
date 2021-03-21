﻿using Player.Input.Action;

namespace Player.Input
{
    public interface IInputHandler
    {
        int MovementDirection { get; }
        IAction CheckInputAction { get; }
        IAction ConsumeCurrentInputAction { get; }
    }
}