using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int hp;
    public int maxHP;
    public bool alive;
    private float invincibilityTime = 1.5f;
    private float timeSinceLastHit;
    public UnityEvent hurtEvent;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHP;
        alive = true;
        timeSinceLastHit = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp < 0)
        {
            alive = false;
        }
    }

    public void TakeDamage()
    {
        if (Time.time - timeSinceLastHit > invincibilityTime)
        {
            hp--;
            timeSinceLastHit = Time.time;
            if (hp >= 0)
                hurtEvent.Invoke();
        }
    }


}
