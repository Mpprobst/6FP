using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    private PlayerController player;
    private Animator animator;
    private Queue<string> animQueue;
    private bool animating;
    private bool rotatePlayer;

    public UnityEvent onComplete;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        animator = GetComponent<Animator>();
    }

    public void Initialize()
    {
        animQueue = new Queue<string>();

        animQueue.Enqueue("RotateLeft");
        animQueue.Enqueue("RotateRight");
        animQueue.Enqueue("Poke");
        animQueue.Enqueue("SwipeLeft");
        animQueue.Enqueue("SwipeRight");
        animQueue.Enqueue("Cough");
        animQueue.Enqueue("Go");
        animQueue.Enqueue("Go");

        StartCoroutine(TutorialCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (rotatePlayer)
        {
            player.Rotate();
        }
    }

    private IEnumerator TutorialCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        while (animQueue.Count > 0)
        {
            yield return new WaitForSeconds(0.25f);

            if (!animating)
            {
                //yield return new WaitForSeconds(0.1f);

                string nextDemo = animQueue.Dequeue();
                Debug.Log(nextDemo);
                animator.SetTrigger(nextDemo);
                StartCoroutine(nextDemo);
                animating = true;
            }
        }
        onComplete.Invoke();
    }

    public IEnumerator RotateLeft()
    {
        Debug.Log("Rotate Left");
        player.tutOverride = true;
        player.inputVal = -1;
        player.rotateCW = true;
        rotatePlayer = true;
        yield return new WaitForSeconds(0.25f);

        ReturnFromRotate();
    }

    public IEnumerator RotateRight()
    {
        Debug.Log("Rotate Right");
        player.tutOverride = true;
        player.inputVal = 1;
        player.rotateCW = false;
        rotatePlayer = true;

        yield return new WaitForSeconds(0.25f);

        ReturnFromRotate();
    }

    private void ReturnFromRotate()
    {
        Debug.Log("Rotate Back");

        rotatePlayer = true;
        if (player.rotateCW)
        {
            player.inputVal = -1;
        }
        else
        {
            player.inputVal = 1;
        }
    }

    public IEnumerator Poke()
    {
        yield return new WaitForSeconds(0.25f);
        player.Poke();
    }

    public IEnumerator SwipeLeft()
    {
        yield return new WaitForSeconds(0.25f);

        player.rotateCW = false;
        player.Swipe();
    }

    public IEnumerator SwipeRight()
    {
        yield return new WaitForSeconds(0.25f);

        player.rotateCW = true;
        player.Swipe();
    }

    public IEnumerator Cough()
    {
        yield return new WaitForSeconds(0.5f);
        player.timeSinceLastCough = -10f;
        player.Cough();
    }

    public IEnumerator Go()
    {
        yield return new WaitForSeconds(1f);

    }

    public void AnimationReturn()
    {
        Debug.Log("Animation Complete");
        player.tutOverride = false;
        player.inputVal = 0;
        rotatePlayer = false;
        animating = false;
    }
}
