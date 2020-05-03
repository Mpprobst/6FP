using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    public SpawnerData spawnerData;
    [System.NonSerialized] public ScoreManager score;
    private bool canSpawn = false;
    //[System.NonSerialized]
    public bool complete = false;

    // Spawner variances
    [Header("Spawner Preferences")]
    public int maxAlive = 3;
    public float spawnFrequency = 2f;
    public List<GameObject[]> locations;
    public float minEnemySpeed = 1;
    public float maxEnemySpeed = 1;

    // Enemy control
    private float timeSinceLastSpawn;
    //[System.NonSerialized]
    public List<EnemyController> activeEnemies;
    private int spawnCount;

    private void Start()
    {
        minEnemySpeed = 1;
        maxEnemySpeed = 1;
        score = FindObjectOfType<ScoreManager>();
        complete = false;
        locations = new List<GameObject[]>();

        GameObject[] directions = GameObject.FindGameObjectsWithTag("Direction");
        for (int i = 0; i < directions.Length; i++)
        {
            Transform[] locationTransforms = directions[i].GetComponentsInChildren<Transform>();
            GameObject[] location = new GameObject[locationTransforms.Length];
            for (int j = 0; j < locationTransforms.Length; j++)
            {
                location[j] = locationTransforms[j].gameObject;
            }
            //GameObject[] location = directions[i].GetComponentsInChildren<GameObject>();
            locations.Add(location);
        }

        timeSinceLastSpawn = Time.time;
        activeEnemies = new List<EnemyController>();
        spawnCount = 0;
    }

    private void Update()
    {
        if ((Time.time - timeSinceLastSpawn) > spawnFrequency && !complete && canSpawn)
        {
            if (activeEnemies.Count < maxAlive)
            {
                // instantiate a new ball
                GameObject prefabToSpawn = ChooseEnemy(spawnerData);

                GameObject obj = (GameObject)Instantiate(prefabToSpawn);
                EnemyController spawnedEnemy = obj.GetComponent<EnemyController>();
                // give each ball a unique ID so that we know which ball to remove from the active array later
                spawnedEnemy.destroyedEvent = new EnemyEvent();
                spawnedEnemy.destroyedEvent.AddListener(EnemyDestroyed);
                spawnedEnemy.ID = spawnCount;

                spawnedEnemy.transform.parent = this.transform;
                int randomDirection = Random.Range(0, locations.Count);
                spawnedEnemy.transform.position = locations[randomDirection][Random.Range(0, locations[randomDirection].Length)].transform.position;

                spawnedEnemy.Initialize();

                if (spawnedEnemy.GetType() == typeof(NeutralController))
                {
                    NeutralController neutralEnemy = spawnedEnemy.GetComponent<NeutralController>();
                    neutralEnemy.goals = locations;
                    neutralEnemy.ChangeGoal(randomDirection);
                }
                spawnedEnemy.movement.speed = Random.Range(minEnemySpeed, maxEnemySpeed);

                activeEnemies.Add(spawnedEnemy);

                spawnCount++;
                timeSinceLastSpawn = Time.time;
            }
            else if (activeEnemies.Count == 0)
            {
                // if the spawnwer has spawned to completion, and there are no active balls, then it is complete
                complete = true;
            }

        }
    }

    public void BeginSpawning()
    {
        Debug.Log("Begin spawning");
        canSpawn = true;
    }

    public void StopSpawning()
    {
        Debug.Log("Stop spawning");
        canSpawn = false;
    }

    /// <summary>
    /// Uses weighted probability of an item to choose which to spawn
    /// </summary>
    private GameObject ChooseEnemy(SpawnerData data)
    {
        float totalProbability = 0;

        int itemIndex;
        for (itemIndex = 0; itemIndex < data.enemies.Length; itemIndex++)
        {
            totalProbability += data.enemies[itemIndex].weightedProbability;
        }

        float randomVal = Random.Range(0, totalProbability);

        for (itemIndex = 0; itemIndex < data.enemies.Length; itemIndex++)
        {
            if (randomVal < data.enemies[itemIndex].weightedProbability)
            {
                return data.enemies[itemIndex].prefab;
            }
            else
            {
                randomVal -= data.enemies[itemIndex].weightedProbability;
            }
        }
        return data.enemies[data.enemies.Length].prefab;
    }

    private void EnemyDestroyed(int ID)
    {
        // remove destroyed ball from the array of active balls
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            if (activeEnemies[i].ID == ID)
            {
                score.AddScore(activeEnemies[i].points);
                activeEnemies.RemoveAt(i);
            }
        }
    }
}
