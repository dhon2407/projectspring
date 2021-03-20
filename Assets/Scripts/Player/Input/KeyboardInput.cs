using System.Collections.Generic;
using Player.Input.Action;
using UnityEngine;

namespace Player.Input
{
    public class KeyboardInput : MonoBehaviour, IInputHandler
    {
        public int MovementDirection => 1;
        public IAction CheckInputAction => _inputActions.Count <= 0 ? null : _inputActions.Peek();
        public IAction ConsumeCurrentInputAction
        {
            get
            {
                if (_inputActions.Count <= 0)
                    return null;
                
                var currentAction =_inputActions.Dequeue();
                _inputActions.Clear();
                return currentAction;
            }
        }

        private readonly Queue<IAction> _inputActions = new Queue<IAction>();

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
                _inputActions.Enqueue(new JumpAction());
        }
    }
}