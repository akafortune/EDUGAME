using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radioactive_ActBox : MonoBehaviour
{
    public Player_Movement playerScript;
    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = parent.transform.position;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && parent.GetComponent<Radioactive_Behavior>().currState != Radioactive_Behavior.Radioactive_State.EXPLODING)
        {
            if (playerScript.actionable && !playerScript.carrying)
            {
                if (Input.GetAxis("Act") > 0 )
                {
                    playerScript.carrying = true;
                    parent.GetComponent<Radioactive_Behavior>().currState = Radioactive_Behavior.Radioactive_State.PICKED_UP;
                    playerScript.carryingItem = parent;
                }
            }

            if(playerScript.carrying && parent.GetComponent<Radioactive_Behavior>().currState == Radioactive_Behavior.Radioactive_State.PICKED_UP)
            {
                if(Input.GetAxis("Throw") > 0 && parent.GetComponent<Radioactive_Behavior>().currState != Radioactive_Behavior.Radioactive_State.EXPLODING)
                {
                    playerScript.carrying = false;
                    SetThrowDir();
                    parent.GetComponent<Radioactive_Behavior>().currState = Radioactive_Behavior.Radioactive_State.THROWN;
                    playerScript.carryingItem = null;
                }
            }
        }
    }

    void SetThrowDir()
    {
        float x = 0, y = 0;

        if(playerScript.facing[0] == Player_Movement.Directions.LEFT)
        {
            x = -1;
        } else if (playerScript.facing[0] == Player_Movement.Directions.RIGHT)
        {
            x = 1;
        }

        if (playerScript.facing[1] == Player_Movement.Directions.DOWN)
        {
            y = -1;
        }
        else if (playerScript.facing[1] == Player_Movement.Directions.UP)
        {
            y = 1;
        }

        Vector3 throwDir = new Vector3(x, y, 0);
        parent.GetComponent<Radioactive_Behavior>().throwDir = throwDir;
    }
}
