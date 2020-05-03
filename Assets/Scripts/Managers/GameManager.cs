using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerController player;
    private ScoreManager scoreManager;
    private SpawnManager spawnManager;
    private UIManager uiManager;
    private bool started = false;
    private Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        player.hurtEvent = new UnityEvent();
        player.hurtEvent.AddListener(CheckPlayer);

        uiManager = GameObject.FindObjectOfType<UIManager>();
        uiManager.InitializeHealthUI(player.health.maxHP);

        scoreManager = GameObject.FindObjectOfType<ScoreManager>();

        spawnManager = GameObject.FindObjectOfType<SpawnManager>();
        camera = GameObject.FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            uiManager.scoreDisplay.text = scoreManager.totalPoints.ToString();
            float coughFill = (Time.time - player.timeSinceLastCough) / player.coughCooldown;
            if (coughFill > 1) coughFill = 1;
            uiManager.coughMeter.transform.localScale = new Vector3(coughFill, 1, 1);
            player.rotationSpeed = player.baseRotationSpeed + spawnManager.difficulty;
        }

    }

    public void CheckPlayer()
    {
        Debug.Log("Check Player");

        for (int i = 2; i > player.health.hp - 1; i--)
        {
            uiManager.healthObjs[i].SetActive(false);
        }
        if (player.dead)
        {
            Invoke("EndGame", 2f);
        }
    }

    public void BeginGame()
    {
        started = true;
        camera.GetComponent<Animator>().SetTrigger("Move");
        spawnManager.Begin();
    }

    public void EndGame()
    {
        uiManager.endScreen.SetActive(true);
        started = false;
        spawnManager.spawner.StopSpawning();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Scene currScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currScene.buildIndex);
    }
}
