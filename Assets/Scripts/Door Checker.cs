using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorChecker : MonoBehaviour
{
    public Cancer_Cell_NK_Bheavior[] requiredClusters;
    bool locked = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (locked)
        {
            if (CheckClusters())
            {
                locked = false;
                this.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    bool CheckClusters()
    {
        for(int i = 0; i <= requiredClusters.Length - 1; i++)
        {
            if(requiredClusters[i].currState == Cancer_Cell_NK_Bheavior.CancerClusterState.DEAD)
            {
                return true;
            }
        }

        return false;
    }
}
