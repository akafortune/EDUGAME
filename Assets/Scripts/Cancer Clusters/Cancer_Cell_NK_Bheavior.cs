using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cancer_Cell_NK_Bheavior : MonoBehaviour
{
    public enum CancerClusterState
    {
        ALIVE,
        BREAKING_DOWN,
        DEAD
    }

    public CancerClusterState currState;
    public GameObject wall;
    public float breakdownTime;
    private float breakdownTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        currState = CancerClusterState.ALIVE;
    }

    // Update is called once per frame
    void Update()
    {
       if(currState == CancerClusterState.ALIVE)
       {
            Alive();
       } 
       else if( currState == CancerClusterState.BREAKING_DOWN)
       {
            BreakingDown();
       }
       else if (currState == CancerClusterState.DEAD)
       {
            Dead();
       }
    }

    void Alive()
    {
        wall.GetComponent<BoxCollider2D>().isTrigger = false;
        wall.GetComponent<SpriteRenderer>().enabled = true;
    }

    void BreakingDown()
    {
        breakdownTimer += Time.deltaTime;

        if(breakdownTimer> breakdownTime)
        {
            breakdownTimer= 0;
            currState = CancerClusterState.DEAD;
        }
    }

    private void Dead()
    {
        wall.GetComponent<BoxCollider2D>().isTrigger = true;
        wall.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Radioactive" && currState == CancerClusterState.ALIVE)
        {
            currState = CancerClusterState.BREAKING_DOWN;
        }
    }
}
