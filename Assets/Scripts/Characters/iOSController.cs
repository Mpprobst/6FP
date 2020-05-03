using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iOSController : PlayerController
{
    private int width;
    private int height;
    private Vector2 swipeStart;
    private float swipeStartTime;
    private float maxSwipeTime = 0.5f;
    private bool swipe;

    // Start is called before the first frame update
    protected override void Start()
    {
        width = Screen.width;
        height = Screen.height;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Touch swipeTouch = touch;

            if (inputVal != 0 && Input.touchCount == 2)
            {
                swipeTouch = Input.GetTouch(1);
            }

            if (swipeTouch.phase == TouchPhase.Began)
            {
                swipeStart = swipeTouch.position;
                swipeStartTime = Time.time;
            }


            if (swipeTouch.phase == TouchPhase.Ended && (Time.time - swipeStartTime < maxSwipeTime))
            { 
                Vector2 swipeEnd = swipeTouch.position;
                Vector2 diff = swipeEnd - swipeStart;
                if (diff.magnitude > 200)
                {
                    Vector2 face = new Vector2(transform.forward.x, transform.forward.z);
                    float swipeAngle = Vector2.Angle(face, diff);
                    Debug.Log("swipe angle = " + swipeAngle);
                    Debug.Log("diff = " + diff + " playerPos = " + face);

                    if (swipeAngle < 45f)
                    {
                        Poke();
                    }
                    else
                    {
                        Debug.Log("diff x face = " + diff.x * face.x);
                        if (diff.x * face.x < 0)
                        {
                            rotateCW = true;
                        }
                        else
                        {
                            rotateCW = false;
                        }

                        Swipe();
                    }
                }
            }

            if (true)
            {
                if (touch.position.x < (float)width / 2.0f)
                {
                    inputVal = -1;
                }
                else
                {
                    inputVal = 1;
                }

                Rotate();

            }
        }
        else
        {
            inputVal = 0;
        }

    }
}
