using System;
using Managers;
using Player;
using Player.Enemy;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;
using Utilities.Helpers;

namespace Level
{
    public class TutorialHandler : MonoBehaviour
    {
        [Required, SerializeField]
        private PlayerController player = null;
        [Required, SerializeField]
        private TutorialNotes message = null;
        [Required, SerializeField]
        private SegmentTrigger jumpPoint = null;
        [Required, SerializeField]
        private SegmentTrigger attackPoint = null;
        [Required, SerializeField]
        private SegmentTrigger blockPoint1 = null;
        [Required, SerializeField]
        private SegmentTrigger blockPoint2 = null;
        [Required, SerializeField]
        private TutorialEnemy banditToBlock = null;

        private void Start()
        {
            GameManager.OnGameStarted += RunTutorial;
            jumpPoint.OnTrigger += ExecuteJumpTutorial;
            attackPoint.OnTrigger += ExecuteAttackTutorial;
            blockPoint1.OnTrigger += PrepareBlockTutorial;
            blockPoint2.OnTrigger += ExecuteBlockTutorial;
        }

        private void PrepareBlockTutorial()
        {
            blockPoint1.OnTrigger -= PrepareBlockTutorial;
            player.LockAllActions();
        }
        
        private void ExecuteBlockTutorial()
        {
            banditToBlock.Attack();
            blockPoint2.OnTrigger -= ExecuteBlockTutorial;
            player.OnStartBlock += OnStartBlock;
            message.Show("SWIPE LEFT TO BLOCK!");
            Time.timeScale = 0.01f;
            player.UnlockBlock();
        }

        private void OnStartBlock()
        {
            player.OnStartBlock -= OnStartBlock;
            message.Hide();
            player.UnlockAttack();
            player.UnlockJump();
            Time.timeScale = 1;

            Action successBlock = OnBlock;
            successBlock.DelayInvoke(0.3f);
        }

        private void OnBlock()
        {
            banditToBlock.ThrowOff();
            player.OnSuccessBlock();
        }

        private void ExecuteAttackTutorial()
        {
            attackPoint.OnTrigger -= ExecuteAttackTutorial;
            player.OnStartAttack += OnStartAttack;
            
            message.Show("TAP TO ATTACK!");
            Time.timeScale = 0.05f;
            player.LockAllActions();
            player.UnlockAttack();
        }

        private void OnStartAttack()
        {
            player.OnStartAttack -= OnStartAttack;
            player.UnlockJump();
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
