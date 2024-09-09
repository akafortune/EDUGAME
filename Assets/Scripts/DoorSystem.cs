using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    public Player_Movement pm;
    public DoorSystem nextDoor;
    public float speed;
    public bool doorTouched;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0.02f;
        doorTouched = false;
    }

    // Update is called once per frame
    void Update()
    {
        // when the player reaches a door to move through to the next
        if (doorTouched && pm.playerTransition == Player_Movement.DoorTransitions.Transitioning)
        {
            MovePlayer();
            print("Player is moving");
        }
        // when the player reaches the next door
        if(NextDoorReached())
        {
            doorTouched = false;
        }
    }

    // when the player reaches the next door point, true is returned
    public bool NextDoorReached()
    {
        if (pm.transform.position == nextDoor.transform.position)
        {
            pm.ChangeTransitionState("NoTransition");
            doorTouched = false;
            print("Door is reached");
            return true;
        }
        else
            return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Player"))
        {
            // begins the transitioning sequence
            if(pm.playerTransition == Player_Movement.DoorTransitions.NoTransition)
            {
                pm.ChangeTransitionState("Transitioning");
                doorTouched = true;
            }
            //else if(pm.playerTransition == Player_Movement.DoorTransitions.Transitioning && NextDoorReached())
            //{
            //    pm.ChangeTransitionState("NoTransition");
            //    doorTouched = false;
            //}
            //else if(pm.playerTransition == Player_Movement.DoorTransitions.Transitioning)
            //{
            //    // move player position to nextDoor position
            //    //pm.gameObject.transform.position = Vector2.Lerp(pm.gameObject.transform.position, nextDoor.gameObject.transform.position, speed);
            //    pm.transform.position = nextDoor.transform.position;
            //    // set playerTransition to hasTransitioned?
            //}

        }
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.tag.Equals("Player"))
    //    {
    //        if (pm.playerTransition == Player_Movement.DoorTransitions.Transitioning)
    //        {
    //            //// move player position to nextDoor position
    //            //print(nextDoor.transform.position + " " + pm.transform.position);
    //            ////pm.transform.position = nextDoor.transform.position;
    //            //pm.ChangeTransitionState("HasTransitioned");
    //            //// set playerTransition to hasTransitioned?
    //        }
    //    }
    //}

    public void MovePlayer()
    {
        pm.transform.position = Vector2.MoveTowards(pm.transform.position, nextDoor.transform.position, speed);
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
