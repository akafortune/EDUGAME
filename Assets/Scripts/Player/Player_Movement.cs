using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using static Basic_Enemy_Behavior;

public class Player_Movement : MonoBehaviour
{
    public enum MovementStates
    {
        STANDING,
        MOVING,
        ROLLING,
        ACTING,
        STUNNING,
        HIT,
        DEAD
    }

    public enum Directions
    {
        NONE,
        UP,
        DOWN,
        LEFT,
        RIGHT,
    }
    public enum DoorTransitions
    {
        NoTransition,
        Transitioning,
        HasTransitioned,
    }

    public Directions[] facing;     //cell 1 is for left/right, cell 2 is for up/down
    public Directions lastPressed = Directions.NONE;
    public MovementStates[] inactionableStates;
    public static MovementStates playerState;
    public Vector3 rollTarget, hitPos, reelTarget;
    public Transform carryPos;
    public bool restand, actionable, intangible, carrying, reeled = false;
    public GameObject stunBox, carryingItem;
    private BoxCollider2D collisionBox;
    public float speed, rollDist, reelSpeed, reelDist, stunDist, rollSpeed, restandTime, rollTime, swingTime, downTime, intangibleTime, hitPosLenience;
    private float restandTimer = 0, rollTimer = 0, swingTimer = 0, downTimer = 0, intangibleTimer = 0;

    private void Start()
    {
        collisionBox = this.gameObject.GetComponent<BoxCollider2D>();
    }
    public DoorTransitions playerTransition = DoorTransitions.NoTransition;

    void Update()
    {
        actionable = ActionCheck();

        collisionBox.isTrigger = false;

        if (intangible)
        {
            IntangibleTimer();
        }

        if(actionable)
        {
            MovementCheck(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));  //Execute Player Movement
            lastPressed = PressCheck();

            if (!carrying)
            {
                if (Input.GetAxis("Roll") > 0)  //Roll Entry Check
                {
                    playerState = MovementStates.ROLLING;
                    SetRollDir();
                }

                if (Input.GetAxis("Stun") > 0)
                {
                    Stun();
                }
            }
        }

        if (playerState == MovementStates.STUNNING)
        {
            swingTimer += Time.deltaTime;

            if (swingTimer > swingTime)
            {
                swingTimer = 0;
                playerState = MovementStates.STANDING;
                stunBox.SetActive(false);
            }
        }

        if (playerState == MovementStates.ROLLING & !restand)
        {
            intangible = true;
            if (Roll())  //Roll returns a bool determining if the player is done rolling (determined via a timer)
            {
                restand = true;
                this.gameObject.layer = 8;
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

        

        if(playerState == MovementStates.HIT)
        {
            if (!reeled)
            {
                SetReelPos();
            }
            stunBox.SetActive(false);
            Hit();
        }

        if(carryingItem != null)
        {
            if (carryingItem.GetComponent<Radioactive_Behavior>().currState == Radioactive_Behavior.Radioactive_State.EXPLODING)
            {
                carrying = false;
            }
        }
    }

    void SetReelPos()
    {
        float x = 0, y = 0;

        if (hitPos.x > this.gameObject.transform.position.x + hitPosLenience)
        {
            x = -1;
        } else if (hitPos.x < this.gameObject.transform.position.x - hitPosLenience)
        {
            x = 1;
        }

        if (hitPos.y > this.gameObject.transform.position.y + hitPosLenience)
        {
            y = -1;
        }
        else if (hitPos.y < this.gameObject.transform.position.y - hitPosLenience)
        {
            y = 1;
        }

        Vector3 reelDir = new Vector3(x*reelDist, y*reelDist, 0f);
        reelTarget = this.gameObject.transform.position + reelDir;
        reeled = true;
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
        this.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, reelTarget, reelSpeed * Time.deltaTime);

        downTimer += Time.deltaTime;

        if(downTimer > downTime)
        {
            downTimer = 0;
            intangible = true;
            playerState = MovementStates.STANDING;
            reeled = false;
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
        this.gameObject.layer = 7;

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


    public void ChangeTransitionState(string t)
    {
        if(t.Equals("NoTransition"))
        {
            playerTransition = DoorTransitions.NoTransition;
            print("No Transition");
        }
        else if (t.Equals("Transitioning"))
        {
            playerTransition = DoorTransitions.Transitioning;
            print("Transitioning");
        }
        else if(t.Equals("HasTransitioned"))
        {
            playerTransition = DoorTransitions.HasTransitioned;
            print("HasTransitioned");
        }
        else
        {
            print("Wrong String: " + t);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Hazard")
        {
            if (!intangible)
            {
                playerState = Player_Movement.MovementStates.HIT;
                hitPos = collision.gameObject.transform.position;
            }
        }
    }
}
