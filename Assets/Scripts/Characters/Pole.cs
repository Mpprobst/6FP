using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    public float hitPower = 10f;
    public bool poking = false;
    public bool swiping = false;

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody otherRb = collision.gameObject.GetComponentInParent<Rigidbody>();
        if (otherRb)
        {
            Movement move = otherRb.GetComponent<Movement>();
            if (move && (poking || swiping))
            {
                Health enemyHP = otherRb.GetComponent<Health>();
                enemyHP.TakeDamage();
                move.Stun();
            }
            Vector3 dir = transform.position - collision.transform.position;
            if (poking)
            {
                Debug.Log("poke");
                otherRb.AddForce(-dir * hitPower * 25);
            }
            else if (swiping)
            {
                dir = new Vector3(dir.x, 0, 0);
                otherRb.AddForce(-dir * hitPower);
            }


        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
