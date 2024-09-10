using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Enemy_Behavior : MonoBehaviour
{
    public enum EnemyState
    {
        PATROLLING, 
        CHASING,
        LOOKING
    }

    public EnemyState enemyState;
    public Transform[] waypoints;
    public GameObject player;
    public float patrolSpeed, chaseSpeed, lookTime;
    private float  lookTimer = 0;
    public int nextWaypoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyState.PATROLLING;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyState== EnemyState.PATROLLING)
        {
            Patrol();
        }

        if(enemyState == EnemyState.CHASING)
        {
            Chase();
        }

        if (enemyState == EnemyState.LOOKING)
        {
            Look();
        }
    }

    void Chase()
    {
        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, player.transform.position, chaseSpeed * Time.deltaTime);
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

}
