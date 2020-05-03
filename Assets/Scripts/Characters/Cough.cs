using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cough : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody otherRb = other.GetComponentInParent<Rigidbody>();
        EnemyController enemy = other.GetComponentInParent<EnemyController>();
        if (otherRb && enemy)
        {
            Debug.Log("Trigger " + enemy.name);

            otherRb.AddExplosionForce(10, transform.position, GetComponent<SphereCollider>().radius);
        }
    }
    private void OnEnable()
    {
        Invoke("Disable", 0.1f);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
