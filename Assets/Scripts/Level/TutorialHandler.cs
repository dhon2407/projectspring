using System;
using System.Collections.Generic;
using Managers;
using MEC;
using Player;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;
using Utilities.Helpers;

namespace Level
{
    public class TutorialHandler : MonoBehaviour
    {
        [Required, SerializeField]
        private PlayerController player;
        [Required, SerializeField]
        private TutorialNotes message;
        [Required, SerializeField]
        private SegmentTrigger jumpPoint;

        private void Start()
        {
            GameManager.OnGameStarted += RunTutorial;
            jumpPoint.OnTrigger += ExecuteJumpTutorial;
        }

        private void ExecuteJumpTutorial()
        {
            player.OnJump += () =>
            {
                Time.timeScale = 1;
                message.Hide();
            };
            
            message.Show("SWIPE UP TO JUMP!");
            Time.timeScale = 0.3f;
            player.UnlockJump();
        }

        private void RunTutorial()
        {
            Action lockActions = player.LockAllActions;
            lockActions.DelayInvoke(0.1f);
        }
    }
}
