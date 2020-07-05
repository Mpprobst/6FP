using Firebase;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class Login : MonoBehaviour
{
    private FirebaseAuth auth;
    private DatabaseReference mDatabaseRef;
    private FirebaseUser fbUser;

    public InputField usernameInput;
    public InputField emailInput;
    public InputField passwordInput;
    public LeaderboardManager leaderboard;

    private bool success;
    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://foot-pole.firebaseio.com/");
        mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    // Update is called once per frame
    void Update()
    {
        if (success)
        {
            Debug.Log("success");
            LoadLeaderBoard();
            success = false;
        }
    }

    public void LoginUser()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        success = false;
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            fbUser = newUser;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            Debug.Log("Logged in!");
            success = true;
        });

    }

    public void RegisterNewUser()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        string username = usernameInput.text;
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            fbUser = newUser;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            writeNewUser(newUser.UserId, email, username);
        });
    }

    private void writeNewUser(string userId, string email, string username)
    {
        User user = new User(username, email);
        string json = JsonUtility.ToJson(user);

        Debug.Log("writing user: " + userId + "with username " + username);
        //Debug.Log(Application.persistentDataPath + "/" + username);
        //File.WriteAllText((Application.persistentDataPath + "/" + username) , json);
        //mDatabaseRef.Child("users").Child(userId).Child("username").SetValueAsync(name);
        //mDatabaseRef.Child("users").Child(userId).Child("highscore").SetValueAsync(0);
        mDatabaseRef.Child("users").Child(userId).SetRawJsonValueAsync(json);
        success = true;
        //string key = mDatabaseRef.Child("users").Push().Key;
    }

    private void LoadLeaderBoard()
    { 
        gameObject.transform.parent.gameObject.SetActive(false);
        leaderboard.gameObject.SetActive(true);
    }
}
