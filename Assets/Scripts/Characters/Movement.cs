using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;

    public float speed = 50;
    public float stunTime = 1f;
    private bool stunned;
    public bool goalMet;

    private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stunned)
        {
            MoveToGoal();
        }
    }

    private void MoveToGoal()
    {
        Vector3 dir = target.transform.position - transform.position;
        float dist = Vector3.Distance(transform.position, target.transform.position);

        if (dist > 0.1f)
        {
            //transform.position = (Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            rb.MovePosition(Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime));
            Quaternion targetRot = Quaternion.LookRotation(target.transform.position - transform.position);
            rb.MoveRotation(targetRot);
            goalMet = false;
        }
        else
        {
            goalMet = true;
        }
    }

    public void Stun()
    {
        stunned = true;
        Invoke("UnStun", stunTime);
    }

    private void UnStun()
    {
        stunned = false;
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }
}
