using Firebase;
using Firebase.Auth;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentUser : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseUser user;
    // Start is called before the first frame update
    void Start()
    {
        CurrentUser[] currentUsers = GameObject.FindObjectsOfType<CurrentUser>();
        if (currentUsers.Length > 1)
        {
            Destroy(gameObject);
        }

        InitializeFirebase();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                //displayName = user.DisplayName ?? "";
                //emailAddress = user.Email ?? "";
                //photoUrl = user.PhotoUrl ?? "";
            }
        }
    }
}
