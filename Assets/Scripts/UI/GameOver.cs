using System;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Touch;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities.Helpers;

namespace UI
{
    [HideMonoScript]
    public class GameOver : MonoBehaviour
    {
        [Required, SerializeField]
        private GameObject mainMessage;
        [Required, SerializeField]
        private GameObject playAgainMessage;
        [Required, SerializeField]
        private CanvasGroup blackCurtain;
        [Required, SerializeField]
        private LeaderBoard leaderBoard;
        
        private CanvasGroup _canvasGroup;
        private EndGameDistanceHandler _distance;
        private EndGamePersonalRecord _personalRecord;
        private bool _readyToRestart;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _distance = GetComponentInChildren<EndGameDistanceHandler>();
            _personalRecord = GetComponentInChildren<EndGamePersonalRecord>();
            
            _canvasGroup.alpha = 0;
            
            GameManager.OnGameEnded += GameEnded;
            GameManager.OnReplayGameReadyIn += ReplayReadyIn;
            
            mainMessage.SetActive(false);
            playAgainMessage.SetActive(false);
        }

        private void ReplayReadyIn(float duration)
        {
            ResetAppearance();

            _canvasGroup.DOFade(0, 0);
            blackCurtain.DOFade(0, duration - 0.3f);
        }

        private void HandleFingerTap(LeanFinger obj)
        {
            if (_readyToRestart)
                PlayAgain();
        }

        private void PlayAgain()
        {
            LeanTouch.OnFingerTap -= HandleFingerTap;
            _readyToRestart = false;
            FadeToReset();
        }

        private void ResetAppearance()
        {
            _personalRecord.Hide();
            mainMessage.SetActive(false);
            playAgainMessage.SetActive(false);
            _distance.Hide();
        }

        private void FadeToReset()
        {
            blackCurtain.DOFade(1, 0.2f)
                .OnComplete(GameManager.PlayAgain);
        }

        private void GameEnded()
        {
            LeanTouch.OnFingerTap += HandleFingerTap;

            mainMessage.SetActive(true);
            _canvasGroup.DOFade(1, 1.5f)
                .OnComplete(ShowResults);
        }

        private void ShowResults()
        {
            ShowDistanceSummary();
        }

        private void ShowDistanceSummary()
        {
            _distance.Show((int)GameManager.DistanceTraveled, ShowRecord);
        }

        private void ShowRecord()
        {
            var distance = GameManager.DistanceTraveled;
            var currentRecord = GameManager.DistancePersonalRecord;
            _personalRecord.Show((int)distance, (int)currentRecord);
            GameManager.UpdateRecord();
            
            LeaderBoardManager.GetScores(HighScoresLoaded);

            Action playAgain = CanPlayAgain;
            playAgain.DelayInvoke(3f);
        }

        private void HighScoresLoaded(List<(string, int)> scores)
        {
            Action action = () =>
            {
                _distance.Hide();
                _personalRecord.Hide();
                mainMessage.SetActive(false);
                leaderBoard.Show(scores);
            };
            
            action.DelayInvoke(2f);
        }

        private void CanPlayAgain()
        {
            _readyToRestart = true;
            playAgainMessage.SetActive(true);
        }
    }
}
