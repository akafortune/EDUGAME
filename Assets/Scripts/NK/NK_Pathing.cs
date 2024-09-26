using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Basic_Enemy_Behavior;

public class NK_Pathing : MonoBehaviour
{
    public enum NK_State
    {
        WAITING,
        WALKING,
        TURNING,
        STUNNED,
        REACHED_TARGET,
        HIT
    }


    public NK_State currState;
    public NK_State[] ineffectiveStates;
    public Transform[] walkingTargets;
    public Rigidbody2D collisionBox;
    private Transform cancerCellTarget;
    public int currTarget = 0;
    public float speed, deathTime, stunTime;
    private float deathTimer = 0, stunTimer = 0;
    public bool effectable;

    public float turnClock = 0, turnTimer;
    // Start is called before the first frame update
    void Start()
    {
        currState = NK_State.WAITING;
        cancerCellTarget = walkingTargets[walkingTargets.Length - 1];
    }

    // Update is called once per frame
    void Update()
    {
        effectable = effectiveCheck();

        if (!effectable)
        {
            collisionBox.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        } else
        {
            collisionBox.constraints = RigidbodyConstraints2D.None;
        }

        if(currState == NK_State.WALKING)
        {
            Walk();
        }

        if(currState == NK_State.TURNING)
        {
            Turn();
        }

        if(currState == NK_State.HIT)
        {
            collisionBox.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            Hit();
        }

        if(currState == NK_State.STUNNED)
        {
            Stun();
        }
       
    }

    public bool effectiveCheck()
    {
        for(int i = 0; i < ineffectiveStates.Length; i++)
        {
            if(currState == ineffectiveStates[i])
            {
                return false;
            }
        }

        return true;
    }

    public void Walk()
    {
        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, walkingTargets[currTarget].position, speed * Time.deltaTime);

        if (this.gameObject.transform.position == walkingTargets[currTarget].position)
        {
            if(currTarget + 1 >= walkingTargets.Length)
            {
                currTarget++;
                currState = NK_State.REACHED_TARGET;
                cancerCellTarget.GetComponent<Cancer_Cell_NK_Bheavior>().currState = Cancer_Cell_NK_Bheavior.CancerClusterState.BREAKING_DOWN;
            } else
            {
                currState = NK_State.TURNING;
            }
        }
        
    }

    public void Stun()
    {
        stunTimer += Time.deltaTime;

        if(stunTimer >= stunTime)
        {
            stunTimer = 0;
            currState = NK_State.WALKING;
        }
    }

    public void Turn()
    {
        turnClock += Time.deltaTime;

        if (turnClock >= turnTimer)
        {
            turnClock = 0;
            currTarget++;
            currState = NK_State.WALKING;
        }
    }

    public void Hit()
    {
        deathTimer += Time.deltaTime;

        if(deathTimer > deathTime)
        {
            deathTimer = 0;
            currTarget = 0;
            this.gameObject.SetActive(false);
            this.gameObject.transform.position = walkingTargets[0].transform.position;
            currState = NK_State.WAITING;
            this.gameObject.SetActive(true);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Stun" && effectable)
        {
            currState = NK_State.STUNNED;
        }
    }
}
