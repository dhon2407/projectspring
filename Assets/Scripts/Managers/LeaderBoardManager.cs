using System.Collections;
using CustomHelper;
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
            SetupDatabase();
        }

        private static LeaderBoardManager Instance =>
            _instance ? _instance : throw new UnityException($"No instance of {nameof(LeaderBoardManager)}");

        private static LeaderBoardManager _instance;
        private FirebaseAuth _auth;

        protected override LeaderBoardManager Self
        {
            set => _instance = value;
            get => _instance;
        }

        #endregion

        [Button(ButtonSizes.Large)]
        private void ForceLogin(string userName)
        {
            StartCoroutine(Login(userName, "123456"));
        }

        private void SetupDatabase()
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

        private void InitializeFirebase()
        {
            _auth = FirebaseAuth.DefaultInstance;
            this.Log($"Firebase initialized. {(_auth != null ? "OK" :"NG")}");
        }

        private IEnumerator Login(string userName, string password)
        {
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
                    yield return CreateLogin(userName, email, password);
                }
                else
                    this.Log($"Login error : {errorCode}");
            }
            else
            {
                var user = loginTask.Result;
                this.Log($"User signed in successfully: {user.DisplayName} ({user.Email})");
            }
        }

        private IEnumerator CreateLogin(string username, string email, string password)
        {
            var registerTask = _auth.CreateUserWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(() => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {registerTask.Exception}");
                FirebaseException firebaseEx = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError) firebaseEx.ErrorCode;

                this.Log($"Register error : {errorCode}");
            }
            else
            {
                var user = registerTask.Result;

                if (user != null)
                {
                    var profile = new UserProfile {DisplayName = username};

                    var profileTask = user.UpdateUserProfileAsync(profile);
                    yield return new WaitUntil(() => profileTask.IsCompleted);

                    if (profileTask.Exception != null)
                        Debug.LogWarning(message: $"Failed to register task with {profileTask.Exception}");
                    else
                        this.Log($"Register {email} successful.");
                }
                else
                {
                    this.Log($"No register task result.");
                }
            }
        }
    }
}