using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiz_Start_Trigger : MonoBehaviour
{
    public Quiz_System qs;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            qs.PrepareField();
        }
    }
}
