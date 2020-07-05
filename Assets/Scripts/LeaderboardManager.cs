﻿using Firebase;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    private DatabaseReference dbReference;
    private FirebaseUser user;
    private FirebaseAuth auth;
    // Start is called before the first frame update
    void Awake()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://foot-pole.firebaseio.com/");
        // For secure login
        //FirebaseApp.DefaultInstance.SetEditorP12FileName("YOUR-FIREBASE-APP-P12.p12");
        //FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail("SERVICE-ACCOUNT-ID@YOUR-FIREBASE-APP.iam.gserviceaccount.com");
        //FirebaseApp.DefaultInstance.SetEditorP12Password("notasecret");

        // Get the root reference location of the database.
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        ScoreManager scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        WriteNewScore(user.UserId, scoreManager.totalPoints);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void WriteNewScore(string userId, int score)
    {
        // Create new entry at /user-scores/$userid/$scoreid and at
        // /leaderboard/$scoreid simultaneously
        string key = dbReference.Child("scores").Push().Key;
        Debug.Log("key is "+key);
        LeaderboardEntry entry = new LeaderboardEntry(userId, score);        
        Dictionary<string, object> entryValues = entry.ToDictionary();

        Dictionary<string, object> childUpdates = new Dictionary<string, object>();
        childUpdates["/scores/" + key] = entryValues;
        childUpdates["/user-scores/" + userId + "/" + key] = entryValues;
        Debug.Log("didnt fail yet!");
        dbReference.UpdateChildrenAsync(childUpdates);
    }
}
