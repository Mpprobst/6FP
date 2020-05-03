using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public float baseRotationSpeed = 100f;
    public float rotationSpeed = 100f;
    public float coughPower = 2000f;
    public float coughRadius = 2.5f;

    protected Rigidbody rb;
    protected Animator animator;
    protected Pole pole;
    public Health health;
    public ParticleSystem particles;
    protected AudioSource speaker;
    public AudioClip coughSound;
    public AudioClip pokeSound;
    public AudioClip swipeSound;
    public AudioClip deathSound;

    private bool animating;
    public float coughCooldown = 10f;
    [System.NonSerialized] public float timeSinceLastCough;
    protected bool rotateCW;
    protected float lastAngle;
    public bool dead;
    public UnityEvent hurtEvent;
    protected bool usingMouse;
    protected float inputVal;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rotationSpeed = baseRotationSpeed;
        usingMouse = true;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        pole = GetComponentInChildren<Pole>();
        health = GetComponentInChildren<Health>();
        health.hurtEvent = new UnityEvent();
        health.hurtEvent.AddListener(Hurt);
        speaker = GetComponent<AudioSource>();

        timeSinceLastCough = Time.time;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!dead)
        {
            HandleInput();
        }
        if (!health.alive && !dead)
        {
            dead = true;
            Die();
        }
    }

    protected virtual void HandleInput()
    {
        if (!animating)
        {
            if (usingMouse) RotateToMouse();
            else Rotate();
        }

        if (Input.GetMouseButtonDown(0))
        {
            usingMouse = true;
        }

        inputVal = Input.GetAxis("Horizontal");
        if (inputVal != 0)
        {
            usingMouse = false;
        }

        if (Input.GetButton("Poke") && !pole.poking)
        {
            Poke();
        }

        if (Input.GetButton("Swipe") && !pole.swiping)
        {
            Swipe();
        }

        if (Input.GetButton("Cough") && (Time.time - timeSinceLastCough > coughCooldown))
        {
            Cough();
        }
    }

    protected virtual void Swipe()
    {
        speaker.clip = swipeSound;
        speaker.Play();
        animating = true;
        pole.swiping = true;
        if (rotateCW)
            animator.SetTrigger("SwipeCW");
        else
            animator.SetTrigger("SwipeCCW");
    }

    protected virtual void Poke()
    {
        speaker.clip = pokeSound;
        speaker.Play();
        animating = true;
        pole.poking = true;
        animator.SetTrigger("Poke");
    }

    protected void RotateToMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.localPosition);
        Vector3 dir = mousePos - playerPos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;

        Quaternion qTo = Quaternion.Euler(new Vector3(0, -angle, 0));
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, qTo, rotationSpeed * Time.deltaTime);

        if (transform.localEulerAngles.y - lastAngle > 0) rotateCW = true;
        else rotateCW = false;

        lastAngle = transform.localEulerAngles.y;
    }

    protected virtual void Rotate()
    {
        usingMouse = false;
        //inputVal = Input.GetAxis("Horizontal");
        if (inputVal > 0) rotateCW = true;
        else rotateCW = false;

        transform.Rotate(new Vector3(0, inputVal * rotationSpeed / 50f, 0));
    }

    public virtual void Hurt()
    {
        hurtEvent.Invoke();
        Cough();
    }

    public virtual void Cough()
    {
        if (Time.time - timeSinceLastCough > coughCooldown)
        {
            speaker.clip = coughSound;
            speaker.Play();
            animator.SetTrigger("Cough");
            if (!particles.isPlaying)
            {
                particles.Play();
            }
            else
            {
                particles.Stop();
                particles.Play();
            }
            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, 5);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponentInParent<Rigidbody>();
                EnemyController enemy = hit.GetComponentInParent<EnemyController>();

                if (rb && enemy)
                {
                    rb.AddExplosionForce(coughPower, explosionPos, 5);
                    enemy.GetComponent<Health>().TakeDamage();
                }
            }
            timeSinceLastCough = Time.time;
        }
    }

    protected virtual void Die()
    {
        speaker.clip = deathSound;
        speaker.Play();
        hurtEvent.Invoke();
        //rb.constraints = RigidbodyConstraints.None;
        //rb.isKinematic = false;
        //rb.useGravity = true;
        animator.SetTrigger("Death");
        Debug.Log("Player is dead");
    }

    public void AnimationReturn()
    {
        pole.poking = false;
        pole.swiping = false;
        animating = false;
    }
}
