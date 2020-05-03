using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private PlayerController player;

    public int totalPoints;
    private float lastSecondTime;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        totalPoints = 0;
        lastSecondTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastSecondTime > 1)
        {
            AddScore(1);
            lastSecondTime = Time.time;
        }
    }

    public void AddScore(int points)
    {
        if (!player.dead)
            totalPoints += points;
    }

}
