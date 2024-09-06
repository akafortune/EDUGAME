using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_ActBox : MonoBehaviour
{
    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = parent.transform.position;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(Player_Movement.playerState == Player_Movement.MovementStates.MOVING || Player_Movement.playerState == Player_Movement.MovementStates.STANDING)
            {
                if(Input.GetAxis("Act") > 0)
                {
                    Player_Movement.playerState = Player_Movement.MovementStates.ACTING;
                    Act();
                }
            }
        }
    }

    private void Act()
    {
        this.GetComponentInParent<NK_Pathing>().currState = NK_Pathing.NK_State.WALKING;
        Player_Movement.playerState = Player_Movement.MovementStates.STANDING;
    }
}
