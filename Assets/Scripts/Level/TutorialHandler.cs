using System;
using Managers;
using Player;
using Player.Enemy;
using Settings;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;
using Utilities.Helpers;
using GameSettings = Settings.Core.Settings;

namespace Level
{
    public class TutorialHandler : MonoBehaviour
    {
        public static event EventHandler OnTutorialFinished;
        
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

        private static TutorialSettings Settings => GameSettings.Tutorial;
        
        private void Start()
        {
            GameManager.OnGameStarted += RunTutorial;
            GameManager.OnReplayGame += ReplayGame;
            jumpPoint.OnTrigger += ExecuteJumpTutorial;
            attackPoint.OnTrigger += ExecuteAttackTutorial;
            blockPoint1.OnTrigger += PrepareBlockTutorial;
            blockPoint2.OnTrigger += ExecuteBlockTutorial;
        }

        private void OnDestroy()
        {
            GameManager.OnGameStarted -= RunTutorial;
            GameManager.OnReplayGame -= ReplayGame;
            jumpPoint.OnTrigger -= ExecuteJumpTutorial;
            attackPoint.OnTrigger -= ExecuteAttackTutorial;
            blockPoint1.OnTrigger -= PrepareBlockTutorial;
            blockPoint2.OnTrigger -= ExecuteBlockTutorial;
        }
        
        private void ReplayGame()
        {
            Destroy(this);
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
            message.Show(Settings.blockingMessage);
            Time.timeScale = Settings.blockingReducedTimeScale;
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
            successBlock.DelayInvoke(Settings.onBlockSuccessDelay);
        }

        private void OnBlock()
        {
            banditToBlock.ThrowOff();
            player.OnSuccessBlock();

            Action showSettings = () => OnTutorialFinished?.Invoke(this, EventArgs.Empty);
            showSettings.DelayInvoke(3f);
        }

        private void ExecuteAttackTutorial()
        {
            attackPoint.OnTrigger -= ExecuteAttackTutorial;
            player.OnStartAttack += OnStartAttack;

            message.Show(Settings.attackingMessage);
            Time.timeScale = Settings.attackingReducedTimeScale;
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
            
            message.Show(Settings.jumpingMessage);
            Time.timeScale = Settings.jumpingReducedTimeScale;
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
            lockActions.DelayInvoke(Settings.startLockActionDelay);
        }
    }
}
