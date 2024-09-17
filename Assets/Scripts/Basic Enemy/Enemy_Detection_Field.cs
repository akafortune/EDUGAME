using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Detection_Field : MonoBehaviour
{
    public GameObject parent;
    public Basic_Enemy_Behavior behavior;

    private void Start()
    {
        behavior = parent.GetComponent<Basic_Enemy_Behavior>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.name == "Player" || collision.name == "NK")
        {
            behavior.chaseTarget = collision.gameObject;
            behavior.otherInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player" || collision.name == "NK")
        {
            behavior.otherInRange = false;
        }
    }
}
