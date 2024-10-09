using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard_Button : MonoBehaviour
{
    public GameObject hazard;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            hazard.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            hazard.SetActive(true);
        }
    }
}
