using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    public Player_Movement pm;
    public DoorSystem nextDoor;
    public float speed;

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
        if(collision.tag.Equals("Player"))
        {
            if(pm.playerTransition == Player_Movement.DoorTransitions.NoTransition)
            {
                pm.ChangeTransitionState("Transitioning");
            }
            if(pm.playerTransition == Player_Movement.DoorTransitions.HasTransitioned)
            {
                pm.ChangeTransitionState("NoTransition");
            }
            //else if(pm.playerTransition == Player_Movement.DoorTransitions.Transitioning)
            //{
            //    // move player position to nextDoor position
            //    //pm.gameObject.transform.position = Vector2.Lerp(pm.gameObject.transform.position, nextDoor.gameObject.transform.position, speed);
            //    pm.transform.position = nextDoor.transform.position;
            //    // set playerTransition to hasTransitioned?
            //}

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            if (pm.playerTransition == Player_Movement.DoorTransitions.Transitioning)
            {
                // move player position to nextDoor position
                //pm.gameObject.transform.position = Vector2.Lerp(pm.gameObject.transform.position, nextDoor.gameObject.transform.position, speed);
                pm.transform.position = nextDoor.transform.position;
                pm.ChangeTransitionState("HasTransitioned");
                // set playerTransition to hasTransitioned?
            }
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.tag.Equals("Player"))
    //    {
    //        if (pm.playerTransition == Player_Movement.DoorTransitions.HasTransitioned)
    //        {
    //            pm.ChangeTransitionState("NoTransition");
    //        }
    //    }
    //}
}
