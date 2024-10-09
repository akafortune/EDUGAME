using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cancer_Cluster_Collision_Handler : MonoBehaviour
{
    public Cancer_Cell_NK_Bheavior behavior;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Radioactive" && behavior.currState == Cancer_Cell_NK_Bheavior.CancerClusterState.ALIVE)
        {
            
            behavior.currState = Cancer_Cell_NK_Bheavior.CancerClusterState.BREAKING_DOWN;
        }
    }
}
