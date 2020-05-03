using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iOSController : PlayerController
{
    private int width;
    private int height;
    private Vector2 swipeStart;

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

            if (touch.phase == TouchPhase.Began)
            {
                swipeStart = touch.position;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                Vector2 swipeEnd = touch.position;
                Vector2 diff = swipeEnd - swipeStart;
                Vector2 face = new Vector2(transform.forward.x, transform.forward.z);
                float swipeAngle = Vector2.Angle(face, diff);
                Debug.Log("swipe angle = " + swipeAngle);
                Debug.Log("diff = " + diff + " playerPos = " + face);

                if (swipeAngle < 20f)
                {
                    Poke();
                }
                else
                {
                    Swipe();
                }

            }

            if (Input.touchCount == 2)
            {
                Poke();
            }

            if (touch.position.x < (float)width/2.0f)
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
}
