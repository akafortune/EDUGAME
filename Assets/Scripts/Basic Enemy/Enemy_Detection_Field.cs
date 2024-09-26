using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Detection_Field : MonoBehaviour
{
    public GameObject parent;
    public Basic_Enemy_Behavior behavior;
    public List<GameObject> objsInTrigger = new List<GameObject>();


    private void Start()
    {
        behavior = parent.GetComponent<Basic_Enemy_Behavior>();
    }

    private void Update()
    {
        if(objsInTrigger.Count == 0)
        {
            behavior.otherInRange = false;
        } else
        {
            behavior.otherInRange = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player" || collision.name == "NK")
        {
            objsInTrigger.Add(collision.gameObject);
            behavior.chaseTarget = collision.gameObject;
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player" || collision.name == "NK")
        {
            objsInTrigger.Remove(collision.gameObject);
        }
    }
}
