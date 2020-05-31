using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string username;
    public string email;
    public int highScore;

    public User()
    {
    }

    public User(string username, string email)
    {
        this.username = username;
        this.email = email;
        this.highScore = 0;
    }

}
