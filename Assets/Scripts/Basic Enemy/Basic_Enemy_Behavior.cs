using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Enemy_Behavior : MonoBehaviour
{
    public enum EnemyState
    {
        PATROLLING, 
        CHASING,
        READYING,
        CHARGING,
        LOOKING,
        STUNNED
    }
    
    public EnemyState enemyState;
    public EnemyState[] lockedInStates;
    public Transform[] waypoints;
    private Vector3 dashTarget;
    public GameObject chaseTarget;
    public float patrolSpeed, chaseSpeed, chargeSpeed, lookTime, stunTime, readyTime, chargeTime, chargeDist;
    private float lookTimer = 0, stunTimer = 0, readyTimer = 0, chargeTimer = 0;
    public int nextWaypoint = 0;
    public bool lockedIn, otherInRange, collisionInCharge;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyState.PATROLLING;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        lockedIn = ActionCheck();

        if(!lockedIn)
        {
            if (otherInRange)
            {
                enemyState = EnemyState.CHASING;
                anim.SetBool("isIdle", false);
            }

            if (enemyState == EnemyState.PATROLLING)
            {
                Patrol();
                anim.SetBool("isIdle", true);
            }

            if (enemyState == EnemyState.CHASING)
            {
                Chase();
                anim.SetBool("isIdle", false);
            }
            if (enemyState == EnemyState.LOOKING)
            {
                Look();
                anim.SetBool("isIdle", true);
            }

        }

        if (enemyState == EnemyState.STUNNED)
        {
            Stun();
        }else 
        {
            if (enemyState == EnemyState.READYING)
            {
                Readying();
            }
            else if (enemyState == EnemyState.CHARGING)
            {
                Charging();
            }
        }

        
        

    }

    bool ActionCheck()
    {
        for(int i = 0; i < lockedInStates.Length; i++)
        {
            if (enemyState == lockedInStates[i])
            {
                return true;
            }
        }

        return false;
    }

    void Stun()
    {
        stunTimer += Time.deltaTime;

        if(stunTimer >= stunTime)
        {
            enemyState = EnemyState.LOOKING;
            stunTime = 0;
        }
    }

    void Chase()
    {
        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, chaseTarget.transform.position, chaseSpeed * Time.deltaTime);

        if(Vector3.Distance(this.gameObject.transform.position, chaseTarget.transform.position) <= chargeDist)
        {
            enemyState = EnemyState.READYING;
        }

        if (!otherInRange)
        {
            enemyState = EnemyState.LOOKING;
        }
    }


    void Readying()
    {
        readyTimer += Time.deltaTime;

        if(readyTimer > readyTime)
        {
            readyTimer = 0;
            dashTarget = chaseTarget.transform.position;
            enemyState = EnemyState.CHARGING;
        }
    }

    void Charging()
    {
        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, dashTarget, chargeSpeed * Time.deltaTime);
        chargeTimer += Time.deltaTime;

        if(Vector3.Distance(this.gameObject.transform.position,dashTarget) < .5 || chargeTimer >= chargeTime)
        {
            chargeTimer = 0;
            enemyState = EnemyState.LOOKING;
        }  else if (collisionInCharge)
        {
            chargeTimer = 0;
            collisionInCharge = false;
            enemyState = EnemyState.LOOKING;
        }
    }

    void Look()
    {
        lookTimer += Time.deltaTime;

        if(lookTimer > lookTime)
        {
            lookTimer = 0;
            enemyState = EnemyState.PATROLLING;

            float minDist = Mathf.Abs(Vector3.Distance(this.gameObject.transform.position, waypoints[0].position));
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (Mathf.Abs(Vector3.Distance(this.gameObject.transform.position, waypoints[i].position)) < minDist)
                {
                    nextWaypoint = i;
                }
            }
        }
    }

    void Patrol()
    {
        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, waypoints[nextWaypoint].position, patrolSpeed * Time.deltaTime);

        if(this.gameObject.transform.position == waypoints[nextWaypoint].position )
        {
            if(nextWaypoint + 1 > waypoints.Length - 1)
            {
                nextWaypoint = 0;
            } else
            {
                nextWaypoint++;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Stun")
        {
            enemyState = EnemyState.STUNNED;
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (enemyState == EnemyState.CHARGING)
        {
            collisionInCharge = true;
        }


        if(collision.gameObject.name == "NK")
        {
            if(collision.gameObject.GetComponent<NK_Pathing>().effectable)
            {
                collision.gameObject.GetComponent<NK_Pathing>().currState = NK_Pathing.NK_State.HIT;
                enemyState = EnemyState.PATROLLING;
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<Player_Movement>().intangible)
        {
            collision.gameObject.GetComponent<HealthSystem>().OnHit(1);
        }
    }
}
