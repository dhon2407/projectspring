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
        private GameObject mainMessage = null;
        [Required, SerializeField]
        private GameObject playAgainMessage = null;
        [Required, SerializeField]
        private CanvasGroup blackCurtain = null;
        [Required, SerializeField]
        private LeaderBoard leaderBoard = null;
        
        private CanvasGroup _canvasGroup;
        private EndGameDistanceHandler _distance;
        private EndGamePersonalRecord _personalRecord;
        private bool _readyToRestart;
        private float _distanceCovered;
        private float _currentRecord;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _distance = GetComponentInChildren<EndGameDistanceHandler>();
            _personalRecord = GetComponentInChildren<EndGamePersonalRecord>();
            
            _canvasGroup.alpha = 0;
            SetCanvasActive(false);
            
            GameManager.OnGameEnded += GameEnded;
            GameManager.OnReplayGameReadyIn += ReplayReadyIn;
            
            mainMessage.SetActive(false);
            playAgainMessage.SetActive(false);
        }

        private void ReplayReadyIn(float duration)
        {
            ResetAppearance();

            SetCanvasActive(false);
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
            leaderBoard.Hide();
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
            SetCanvasActive(true);
            ShowDistanceSummary();
        }

        private void ShowDistanceSummary()
        {
            SetupScores();
            _distance.Show((int)GameManager.DistanceTraveled, ShowRecord);
        }

        private void SetupScores()
        {
            _distanceCovered = GameManager.DistanceTraveled;
            _currentRecord = GameManager.DistancePersonalRecord;
            GameManager.UpdateRecord();
        }

        private void ShowRecord()
        {
            _personalRecord.Show((int)_distanceCovered, (int)_currentRecord);
            LeaderBoardManager.GetScores(HighScoresLoaded);

            Action playAgain = CanPlayAgain;
            playAgain.DelayInvoke(5f);
        }

        private void HighScoresLoaded(List<(string, int)> scores, int rank)
        {
            Action action = () =>
            {
                _distance.Hide();
                _personalRecord.Hide();
                mainMessage.SetActive(false);
                leaderBoard.Show(scores, rank);
            };
            
            action.DelayInvoke(4f);
        }

        private void CanPlayAgain()
        {
            _readyToRestart = true;
            playAgainMessage.SetActive(true);
        }
        
        private void SetCanvasActive(bool active)
        {
            _canvasGroup.interactable = active;
            _canvasGroup.blocksRaycasts = active;
        }
    }
}
