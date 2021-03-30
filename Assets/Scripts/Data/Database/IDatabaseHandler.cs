using System;

namespace Data.Database
{
    public enum OperationResult { Success, Fail };

    public interface IDatabaseHandler
    {
        void Login(string nameLabel, Action<OperationResult> resultAction = null);
        void SaveScore(string nameLabel, int score, Action<OperationResult> resultAction = null);
        void FetchHighScore(Action<int> highScore);
        void Setup();
    }
}