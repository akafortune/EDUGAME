using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public enum EnemyState
    {
        PATROLLING, 
        CHASING,
        LOOKING,
        RETURNING
    }

    public EnemyState enemyState;
    public int nextWaypoint = 0;
    public Transform[] waypoints;
    public float speed;

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
    }

    void Patrol()
    {
        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, waypoints[nextWaypoint].position, speed * Time.deltaTime);

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
