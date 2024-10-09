using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard_Button : MonoBehaviour
{
    public GameObject hazard;
    public SpriteRenderer buttonRender;

    public Sprite buttonOn;
    public Sprite buttonOff;
    
    private AudioSource source;
    public AudioClip sfx;
    private void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            source.PlayOneShot(sfx);
        }
    }

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
            source.PlayOneShot(sfx);
            hazard.SetActive(true);
            buttonRender.sprite = buttonOn;
        }
    }
}
