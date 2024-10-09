using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard_Button : MonoBehaviour
{
    public GameObject hazard;
    public SpriteRenderer buttonRender;

    public Sprite buttonOn;
    public Sprite buttonOff;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            hazard.SetActive(false);
            buttonRender.sprite = buttonOff;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            hazard.SetActive(true);
            buttonRender.sprite = buttonOn;
        }
    }
}
