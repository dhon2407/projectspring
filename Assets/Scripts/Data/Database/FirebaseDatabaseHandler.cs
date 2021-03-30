using System;
using System.Collections;
using CustomHelper;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

namespace Data.Database
{
    public class FirebaseDatabaseHandler : MonoBehaviour, IDatabaseHandler
    {
        private const string CommonPass = "123456";
        
        private FirebaseAuth _auth;
        private FirebaseUser _currentUser;
        private DatabaseReference _dbRef;

        public void Setup()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                    InitializeFirebase();
                else
                    Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            });
        }
        
        public void Login(string nameLabel, Action<OperationResult> resultAction)
        {
            StartCoroutine(Login(nameLabel, CommonPass, resultAction));
        }

        public void SaveScore(string nameLabel, int score, Action<OperationResult> resultAction)
        {
            if (_currentUser == null)
                resultAction(OperationResult.Fail);

            StartCoroutine(UpdateHighScore(score));
        }

        public void FetchHighScore(Action<int> highScore)
        {
            StartCoroutine(GetUserHighScore(highScore));
        }

        private void InitializeFirebase()
        {
            _auth = FirebaseAuth.DefaultInstance;
            _dbRef = FirebaseDatabase.DefaultInstance.RootReference;
            this.Log($"Auth initialized. {(_auth != null ? "OK" : "NG")}");
        }

        private IEnumerator Login(string userName, string password, Action<OperationResult> resultAction)
        {
            _currentUser = null;
            var email = $"{userName}@{SystemInfo.deviceUniqueIdentifier}.{Application.productName}";
            var loginTask = _auth.SignInWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(() => loginTask.IsCompleted);

            if (loginTask.Exception != null)
            {
                this.Log(message: $"Failed to register task with {loginTask.Exception}");
                var firebaseEx = loginTask.Exception.GetBaseException() as FirebaseException;
                var errorCode = (AuthError) firebaseEx.ErrorCode;

                if (errorCode == AuthError.UserNotFound)
                {
                    this.Log($"Creating new user {email}");
                    yield return CreateLogin(userName, email, password, resultAction);
                }
                else
                {
                    this.Log($"Login error : {errorCode}");
                    resultAction(OperationResult.Fail);
                }
            }
            else
            {
                _currentUser = loginTask.Result;
                this.Log($"User signed in successfully: {_currentUser.DisplayName} ({_currentUser.Email})");
                resultAction(OperationResult.Success);
            }
        }

        private IEnumerator CreateLogin(string username, string email, string password, Action<OperationResult> resultAction)
        {
            var registerTask = _auth.CreateUserWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(() => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {registerTask.Exception}");
                FirebaseException firebaseEx = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError) firebaseEx.ErrorCode;

                this.Log($"Register error : {errorCode}");
                resultAction(OperationResult.Fail);
            }
            else
            {
                _currentUser = registerTask.Result;

                if (_currentUser != null)
                {
                    var profile = new UserProfile {DisplayName = username};

                    var profileTask = _currentUser.UpdateUserProfileAsync(profile);
                    yield return new WaitUntil(() => profileTask.IsCompleted);

                    if (profileTask.Exception != null)
                        Debug.LogWarning(message: $"Failed to register task with {profileTask.Exception}");
                    else
                        this.Log($"Register {email} successful.");

                    resultAction(profileTask.Exception != null ? OperationResult.Fail : OperationResult.Success);
                }
                else
                {
                    this.LogWarning($"No register task result.");
                    resultAction(OperationResult.Fail);
                }
            }
        }
        
        private IEnumerator UpdateHighScore(int highScore)
        {
            var dbTask = _dbRef.Child("users").Child(_currentUser.UserId).Child("highScore").SetValueAsync(highScore);

            yield return new WaitUntil(() => dbTask.IsCompleted);

            if (dbTask.Exception != null)
                Debug.LogWarning(message: $"Failed to register task with {dbTask.Exception}");
            else
                Debug.Log(message: $"High score {highScore} update for {_currentUser.DisplayName}");
        }

        private IEnumerator GetUserHighScore(Action<int> resultScore)
        {
            var dbTask = _dbRef.Child("users").Child(_currentUser.UserId).GetValueAsync();

            yield return new WaitUntil(() => dbTask.IsCompleted);

            if (dbTask.Exception != null)
                resultScore(-1);
            else if (dbTask.Result.Value == null)
                resultScore(0);
            else
            {
                var snapshot = dbTask.Result;
                int.TryParse(snapshot.Child("highScore").Value.ToString(), out var defaultScore);
                resultScore(defaultScore);
            }
        }
    }
}