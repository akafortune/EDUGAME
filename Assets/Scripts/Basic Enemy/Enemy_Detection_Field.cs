using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Detection_Field : MonoBehaviour
{
    public GameObject parent;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            parent.GetComponent<Basic_Enemy_Behavior>().chaseTarget = collision.gameObject;
            parent.GetComponent<Basic_Enemy_Behavior>().enemyState = Basic_Enemy_Behavior.EnemyState.CHASING;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            parent.GetComponent<Basic_Enemy_Behavior>().enemyState = Basic_Enemy_Behavior.EnemyState.LOOKING;
        }
    }
}
