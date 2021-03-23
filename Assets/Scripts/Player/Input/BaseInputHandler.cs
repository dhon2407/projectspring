using System.Collections.Generic;
using Player.Input.Action;
using UnityEngine;

namespace Player.Input
{
    public abstract class BaseInputHandler : MonoBehaviour, IInputHandler
    {
        public abstract int MovementDirection { get; }
        public IAction CheckInputAction => InputActions.Count <= 0 ? null : InputActions.Peek();
        public IAction ConsumeCurrentInputAction
        {
            get
            {
                if (InputActions.Count <= 0)
                    return null;
                
                var currentAction =InputActions.Dequeue();
                InputActions.Clear();
                return currentAction;
            }
        }

        protected readonly Queue<IAction> InputActions = new Queue<IAction>();
    }
}