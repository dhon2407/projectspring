using System;
using System.Collections;
using System.Collections.Generic;
using CustomHelper;
using Data.Database;
using Firebase;
using Firebase.Auth;
using Managers.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Managers
{
    public class LeaderBoardManager : SingletonManager<LeaderBoardManager>
    {
        #region Singleton Attributes

        protected override void Init()
        {
            _database = GetComponent<IDatabaseHandler>();
            _database.Setup();
        }

        private static LeaderBoardManager Instance =>
            _instance ? _instance : throw new UnityException($"No instance of {nameof(LeaderBoardManager)}");

        private static LeaderBoardManager _instance;

        protected override LeaderBoardManager Self
        {
            set => _instance = value;
            get => _instance;
        }

        #endregion
        
        private IDatabaseHandler _database;

        public static void SaveScore(string name, int score)
        {
            Instance.UpdateScore(name, score);
        }

        public static void FetchHighScore(Action<int> highScoreCallback)
        {
            Instance.GetHighScore(highScoreCallback);
        }

        public static void GetScores(Action<List<(string, int)>> scoreCallback)
        {
            Instance.GetLeaderBoard(scoreCallback);
        }
        
        public static int GetRank(int score)
        {
            return -1;
        }
        
        private void UpdateScore(string nameLabel, int score)
        {
            void LoginAction(OperationResult result)
            {
                if (result == OperationResult.Success)
                    _database.SaveScore(nameLabel, score);
            }

            _database.Login(nameLabel, LoginAction);
        }
        
        private void GetHighScore(Action<int> highScoreCallback)
        {
            _database.FetchHighScore(highScoreCallback);
        }
        
        private void GetLeaderBoard(Action<List<(string, int)>> scoreCallback)
        {
            _database.FetchLeaderBoards(scoreCallback);
        }
    }
}