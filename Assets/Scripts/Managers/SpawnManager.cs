using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Spawner spawner;
    private float timeSinceLastIncrease;
    private float timeToIncrease = 5f;
    public int difficulty = 1;
    private ScoreManager score;
    private bool spawning;

    private void Start()
    {
        spawning = false;
        difficulty = 1;
        spawner = GameObject.FindObjectOfType<Spawner>();
        timeSinceLastIncrease = Time.time;
        score = GameObject.FindObjectOfType<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - timeSinceLastIncrease > timeToIncrease && spawning)
        {
            spawner.maxAlive++;

            spawner.spawnFrequency -= difficulty / 100;
            if (spawner.spawnFrequency < 0.5f) spawner.spawnFrequency = 0.5f; 

            difficulty++;
            score.AddScore(10 * difficulty);
            if (difficulty > 100)
            {
                difficulty = 100;
            }
            spawner.maxEnemySpeed = 1 + (float)difficulty / 20;
            timeSinceLastIncrease = Time.time;
        }
    }

    public void Begin()
    {
        spawning = true;
        spawner.BeginSpawning();
        timeSinceLastIncrease = Time.time;
    }
}
