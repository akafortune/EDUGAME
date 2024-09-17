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
        ACTING,
        STUNNING,
        HIT
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
    public Directions lastPressed = Directions.NONE;
    public MovementStates[] inactionableStates;
    public static MovementStates playerState;
    public Vector3 rollTarget;
    public bool restand, actionable, intangible;
    public GameObject stunBox;
    private BoxCollider2D collisionBox;
    public float speed, rollDist, stunDist, rollSpeed, restandTime, rollTime, swingTime, downTime, intangibleTime;
    private float restandTimer = 0, rollTimer = 0, swingTimer = 0, downTimer = 0, intangibleTimer = 0;

    private void Start()
    {
        collisionBox = this.gameObject.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        actionable = ActionCheck();

        if (intangible)
        {
            IntangibleTimer();
            collisionBox.isTrigger = true;
        } else
        {
            collisionBox.isTrigger = false;
        }

        if(actionable)
        {
            MovementCheck(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));  //Execute Player Movement
            lastPressed = PressCheck();
            
            if (Input.GetAxis("Roll") > 0)  //Roll Entry Check
            {
                playerState = MovementStates.ROLLING;
                SetRollDir();
            }

            if(Input.GetAxis("Stun") > 0)
            {
                Stun();
            }
        }

        if (playerState == MovementStates.ROLLING & !restand)
        {
            intangible = true;
            if (Roll())  //Roll returns a bool determining if the player is done rolling (determined via a timer)
            {
                restand = true;
            }
        }

        if (restand)  //Restand is the time that the player recovers from the end of a roll, only around .1 sec, more of a game feel thing / prevents infinitely chaining rolls
        {
            intangible = false;
            restandTimer += Time.deltaTime;

            if (restandTimer > restandTime)
            {
                restandTimer = 0;
                playerState = MovementStates.STANDING;
                restand = false;
            }
        }

        if(playerState == MovementStates.STUNNING)
        {
            swingTimer += Time.deltaTime;

            if(swingTimer > swingTime)
            {
                swingTimer = 0;
                playerState = MovementStates.STANDING;
                stunBox.SetActive(false);
            }
        }

        if(playerState == MovementStates.HIT)
        {
            stunBox.SetActive(false);
            Hit();
        }
    }

    void IntangibleTimer()
    {
        intangibleTimer += Time.deltaTime;

        if(intangibleTimer >= intangibleTime)
        {
            intangible = false;
            intangibleTime = 0;
        }
    }

    public void Hit()
    {
        downTimer += Time.deltaTime;

        if(downTimer > downTime)
        {
            downTimer = 0;
            intangible = true;
            playerState = MovementStates.STANDING;
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

    public bool ActionCheck()  //Method for compiling all inactionable states into one bool, IF YOU ADD AN INACTIONABLE STATE TO THE FSM PLS ADD A CHECK HERE
    {
        for(int i = 0; i < inactionableStates.Length; i++)
        {
            if(playerState == inactionableStates[i])
            {
                return false;
            }
        }

        return true;
    }

    public Directions PressCheck()
    {
        if(Input.GetAxis("Horizontal") > 0)
        {
            return Directions.RIGHT;
        } else if(Input.GetAxis("Horizontal") < 0)
        {
            return Directions.LEFT;
        } else if (Input.GetAxis("Vertical") > 0)
        {
            return Directions.UP;
        } else if (Input.GetAxis("Vertical") < 0)
        {
            return Directions.DOWN;
        } else
        {
            return lastPressed;
        }
    } //Stores the last direction the player pressed, useful for determining where the stun hitbox goes

    public void Stun()
    {
        playerState = MovementStates.STUNNING;

        int x = 0, y = 0;

        if(lastPressed == Directions.LEFT)
        {
            x = - 1;
        } else if (lastPressed == Directions.RIGHT)
        {
            x = 1;
        }else if (lastPressed == Directions.UP)
        {
            y = 1;
        }else if (lastPressed == Directions.DOWN)
        {
            y = -1;
        }

        stunBox.transform.position = this.gameObject.transform.position + new Vector3(stunDist * x, stunDist * y, 0);
        stunBox.SetActive(true);
    }

    public void SetState(MovementStates newState)
    {
        playerState = newState;
    }

}
