using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public enum MovementStates
    {
        STANDING,
        MOVING,
        ROLLING,
        ACTING
    }

    public enum Directions
    {
        NONE,
        UP,
        DOWN,
        LEFT,
        RIGHT,
    }

    public Directions[] facing;     //cell 1 is for left/right, cell 2 is for up/down
    public static MovementStates playerState;
    public Vector3 rollTarget;
    public bool restand;
    public float speed, rollDist, rollSpeed, restandTime, rollTime;
    private float restandTimer = 0, rollTimer = 0;
    void Update()
    {
        if(playerState != MovementStates.ACTING)
        {
            if (playerState != MovementStates.ROLLING)
            {
                MovementCheck(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            }

            if (Input.GetAxis("Roll") > 0 && playerState != MovementStates.ROLLING)
            {
                playerState = MovementStates.ROLLING;
                SetRollDir();
            }

            if (playerState == MovementStates.ROLLING & !restand)
            {
                if (Roll())
                {
                    restand = true;
                }
            }

            if (restand)
            {
                restandTimer += Time.deltaTime;

                if (restandTimer > restandTime)
                {
                    restandTimer = 0;
                    playerState = MovementStates.STANDING;
                    restand = false;
                }
            }
        }
    }

    void MovementCheck(float x, float y)
    {
        if( x <  0 )
        {
            x = -1;
            facing[0] = Directions.LEFT;
        } else if (x > 0)
        {
            x = 1;
            facing[0] = Directions.RIGHT;
        } else 
        { 
            x = 0;
            facing[0] = Directions.NONE;
        }

        if (y < 0)
        {
            y = -1;
            facing[1] = Directions.DOWN;
        }
        else if (y > 0)
        {
            y = 1;
            facing[1] = Directions.UP;
        }
        else
        {
            y = 0;
            facing[1] = Directions.NONE;
        } 

        
        Vector3 movePos = new Vector3(x * Time.deltaTime, y * Time.deltaTime, 0).normalized;
        this.gameObject.transform.position += movePos * speed;
        
        if(x == 0 && y == 0)
        {
            playerState = MovementStates.STANDING;
        } else
        {
            playerState = MovementStates.MOVING;
        }
        
    }

    public void SetRollDir()
    {
        int x = 0, y = 0;

        if (facing[0] == Directions.LEFT)
        {
            x = -1;
        } else if(facing[0] == Directions.RIGHT)
        {
            x = 1;
        } else if (facing[0] == Directions.NONE)
        {
            x = 0;
        }

        if (facing[1] == Directions.DOWN)
        {
            y = -1;
        }
        else if (facing[1] == Directions.UP)
        {
            y = 1;
        }
        else if (facing[1] == Directions.NONE)
        {
            y = 0;
        }

        if(x == 0 && y == 0)
        {
            playerState = MovementStates.STANDING;
        }

        rollTarget = gameObject.transform.position + new Vector3(x * rollDist, y * rollDist, 0).normalized * rollDist;
        
    }

    public bool Roll()
    {
        rollTimer += Time.deltaTime;
        this.transform.position = Vector3.MoveTowards(this.transform.position, rollTarget, rollSpeed * Time.deltaTime);

        if(rollTimer > rollTime)
        {
            rollTimer = 0;
            return true;
        } else 
        { 
            return false; 
        }
    }

}
