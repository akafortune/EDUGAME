using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard_Switch : MonoBehaviour
{
    public GameObject Hazard;
    public bool timed;
    private bool downed;
    public float downTime;
    private float downTimer = 0;


    private void Update()
    {
        if(downed)
        {
            Hazard.SetActive(false);
            downTimer += Time.deltaTime;

            if(downTimer >= downTime)
            {
                downTimer = 0;
                downed = false;
                Hazard.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Stun")
        {
            if (!timed)
            {
                Hazard.SetActive(!Hazard.activeInHierarchy);
            } else if(timed)
            {
                downed = true;
            }
        }
    }

}
