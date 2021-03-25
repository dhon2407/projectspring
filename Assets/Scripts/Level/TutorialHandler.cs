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
        [Required, SerializeField]
        private SegmentTrigger attackPoint;

        private void Start()
        {
            GameManager.OnGameStarted += RunTutorial;
            jumpPoint.OnTrigger += ExecuteJumpTutorial;
            attackPoint.OnTrigger += ExecuteAttackTutorial;
        }

        private void ExecuteAttackTutorial()
        {
            attackPoint.OnTrigger -= ExecuteAttackTutorial;
            player.OnStartAttack += OnStartAttack;
            
            message.Show("TAP TO ATTACK!");
            Time.timeScale = 0.05f;
            player.UnlockAttack();
        }

        private void OnStartAttack()
        {
            player.OnStartAttack -= OnStartAttack;
            Time.timeScale = 1;
            message.Hide();
        }

        private void ExecuteJumpTutorial()
        {
            jumpPoint.OnTrigger -= ExecuteJumpTutorial;
            player.OnStartJump += OnStartJump;
            
            message.Show("SWIPE UP TO JUMP!");
            Time.timeScale = 0.3f;
            player.UnlockJump();
        }

        private void OnStartJump()
        {
            player.OnStartJump -= OnStartJump;
            Time.timeScale = 1;
            message.Hide();
        }

        private void RunTutorial()
        {
            Action lockActions = player.LockAllActions;
            lockActions.DelayInvoke(0.1f);
        }
    }
}
