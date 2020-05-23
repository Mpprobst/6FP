using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    public GameObject goal;
    public Movement movement;
    protected Health health;
    protected Animator animator;
    public GameObject pointPopupPrefab;
    protected AudioSource speaker;
    public AudioClip[] hurtSounds;
    public AudioClip[] deathSounds;
    public bool idle = false;

    [System.NonSerialized] public int ID;
    public EnemyEvent destroyedEvent;
    public int points;
    private bool dead;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
        Invoke("DelayDestroy", 60f);
        health = GetComponent<Health>();
        health.hurtEvent = new UnityEvent();
        health.hurtEvent.AddListener(TakeDamage);
        movement = GetComponent<Movement>();
        animator = GetComponent<Animator>();
        speaker = GetComponent<AudioSource>();

        if (idle)
        {
            Debug.Log("play idle anim");
            animator.SetBool("Idle", true);
        }

        if (!goal)
        {
            PlayerController pc = GameObject.FindObjectOfType<PlayerController>();
            SetGoal(pc.gameObject);
        }
        else
            movement.SetTarget(goal);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!health.alive && !dead)
        {
            dead = true;
            Die();
        }
    }

    protected virtual void SetGoal(GameObject go)
    {
        goal = go;
        if (movement)
            movement.SetTarget(go);
    }

    protected virtual void Die()
    {
        AudioClip randClip = deathSounds[Random.Range(0, hurtSounds.Length-1)];
        speaker.clip = randClip;
        speaker.Play();
        if (destroyedEvent != null)
            destroyedEvent.Invoke(ID);
        GameObject popup =  (GameObject)Instantiate(pointPopupPrefab);
        popup.GetComponent<PointPopup>().Initialized(points);
        animator.SetBool("Dead", true);
        Invoke("DelayDestroy", 1.5f);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player)
        {
            player.health.TakeDamage();
        }
    }

    protected virtual void TakeDamage()
    {
        AudioClip randClip = hurtSounds[Random.Range(0, hurtSounds.Length-1)];
        speaker.clip = randClip;
        speaker.Play();
        animator.SetTrigger("Hit");
    }

    protected virtual void DelayDestroy()
    {
        if (destroyedEvent != null)
            destroyedEvent.Invoke(ID);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log("destroy ID " + ID);
        //destroyedEvent.Invoke(ID);
    }
}
