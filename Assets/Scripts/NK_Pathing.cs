using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_Pathing : MonoBehaviour
{
    public enum NK_State
    {
        WAITING,
        WALKING,
        TURNING,
        STUNNED,
        REACHED_TARGET,
        DEAD
    }

    public Transform[] walkingTargets;
    public int currTarget = 0;
    public float speed;
    public Rigidbody2D collisionBox;
    public NK_State currState;

    public float turnClock = 0, turnTimer;
    // Start is called before the first frame update
    void Start()
    {
        currState = NK_State.WAITING;
    }

    // Update is called once per frame
    void Update()
    {
        if (currState == NK_State.WAITING || currState == NK_State.REACHED_TARGET)
        {
            collisionBox.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        } else
        {
            collisionBox.constraints = RigidbodyConstraints2D.None;
        }

        Walk();
    }

    public void Walk()
    {
        

        if(currState == NK_State.WALKING)
        {
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, walkingTargets[currTarget].position, speed * Time.deltaTime);

            if (this.gameObject.transform.position == walkingTargets[currTarget].position)
            {
                if(currTarget + 1 >= walkingTargets.Length)
                {
                    currTarget++;
                    currState = NK_State.REACHED_TARGET;
                } else
                {
                    currState = NK_State.TURNING;
                }
            }
        }

        if(currState == NK_State.TURNING)
        {
            turnClock += Time.deltaTime;

            if(turnClock >= turnTimer)
            {
                turnClock = 0;
                currTarget++;
                currState = NK_State.WALKING;
            }
        }
    }

    public void ReceiveAction()
    {
        Debug.Log("heard");
    }
}
