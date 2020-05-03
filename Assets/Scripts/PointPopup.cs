using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointPopup : MonoBehaviour
{
    public Text valueText;
    //public Animator animator;

    public void Initialized(int value)
    {
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        transform.parent = canvas.transform;
        transform.position = Camera.main.WorldToScreenPoint(transform.position);
        valueText.text = "+" + value.ToString();
        Invoke("DelayDestroy", 1f);
    }

    private void DelayDestroy()
    {
        Destroy(gameObject);
    }
}
