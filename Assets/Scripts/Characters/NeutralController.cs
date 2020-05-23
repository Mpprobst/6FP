using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NeutralController : EnemyController
{
    public List<GameObject[]> goals;
    private int currDirection;
    private float timeSinceLastGoal;
    private float timeBetweenGoals = 30f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void Initialize()
    {
        movement = GetComponent<Movement>();
        movement.SetTarget(goal);

        Invoke("DelayDestroy", 60f);
        health = GetComponent<Health>();
        health.hurtEvent = new UnityEvent();
        health.hurtEvent.AddListener(TakeDamage);
        animator = GetComponent<Animator>();
        speaker = GetComponent<AudioSource>();
        timeSinceLastGoal = Time.time;

        if (idle)
        {
            Debug.Log("play idle anim");
            animator.SetBool("Idle", true);
        }

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Time.time - timeSinceLastGoal > timeBetweenGoals)
        {
            ChangeGoal(currDirection);
            timeSinceLastGoal = Time.time;
        }

        if (movement.goalMet)
        {
            ChangeGoal(currDirection);
        }
    }

    public void ChangeGoal(int fromDirection)
    {
        if (goals.Count > 0)
        {
            int otherDirection = 0;
            if (fromDirection == 0)
                otherDirection = 1;
            else if (fromDirection == 2)
                otherDirection = 3;
            else if (fromDirection == 3)
                otherDirection = 2;

            goal = goals[otherDirection][Random.Range(0, goals[fromDirection].Length)];
            currDirection = otherDirection;
            movement.SetTarget(goal);
            timeSinceLastGoal = Time.time;
        }
    }

    protected override void TakeDamage()
    {
        base.TakeDamage();
    }

}
