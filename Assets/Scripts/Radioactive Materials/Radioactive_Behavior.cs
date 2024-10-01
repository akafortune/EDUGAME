using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radioactive_Behavior : MonoBehaviour
{
    public enum Radioactive_State
    {
        GROUND,
        PICKED_UP,
        THROWN,
        EXPLODING
    }

    public Radioactive_State currState;
    private Radioactive_State nextState;
    public GameObject player, actBox, explosionBox;
    private Vector3 originPos;
    public Vector3 throwDir, trueScale;
    public BoxCollider2D barrelCollider;
    public float throwTime, throwDist, scaleValue, decayTime, explosionTime;
    private float throwTimer = 0, decayTimer = 0, explosionTimer = 0;
    private bool rise = true, plucked = false, explosionFirstLoop = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        trueScale = this.gameObject.transform.localScale;
        originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        StateDeterminer();


        if(currState == Radioactive_State.PICKED_UP)
        {
            PickedUp();
        }

        if(currState == Radioactive_State.GROUND)
        {
            Ground();
        }

        if(currState == Radioactive_State.THROWN)
        {
            Thrown();
        }
        
        if(currState == Radioactive_State.EXPLODING)
        {
            Exploding();
        }
    }

    void StateDeterminer()
    {
        if (plucked)
        {
            decayTimer+= Time.deltaTime;

            if(decayTimer > decayTime)
            {
                if (currState == Radioactive_State.THROWN)
                {
                    nextState = Radioactive_State.EXPLODING;
                } else
                {
                    nextState = Radioactive_State.GROUND;
                    currState = Radioactive_State.EXPLODING;
                }
            }
        }
    }

    void PickedUp()
    {
        this.gameObject.transform.position = player.GetComponent<Player_Movement>().carryPos.position;
        barrelCollider.isTrigger = true;
        plucked = true;
        this.gameObject.layer = 8;
    }

    void Ground()
    {
        this.gameObject.layer = 8;
        actBox.SetActive(true);
        barrelCollider.isTrigger = false;
    }

    void Thrown()
    {
        actBox.SetActive(false);

        this.gameObject.layer = 7;

        barrelCollider.isTrigger = false;

        throwTimer += Time.deltaTime;

        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 10);

        this.gameObject.transform.position += throwDir * Time.deltaTime * throwDist;

        if (rise)
        {
            this.gameObject.transform.localScale += new Vector3(scaleValue * Time.deltaTime, scaleValue * Time.deltaTime, 0);

            if (throwTimer >= throwTime / 2.0)
            {
                rise = false;
            }
        } else
        {
            this.gameObject.transform.localScale -= new Vector3(scaleValue * Time.deltaTime, scaleValue * Time.deltaTime, 0);
        }

        

        if(throwTimer >= throwTime)
        {
            throwTimer = 0;
            currState = nextState;
            rise = true;
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0);
            this.gameObject.transform.localScale = trueScale;
        }
    }

    void Exploding()
    {
        this.gameObject.layer = 8;
        if (!explosionFirstLoop)
        {
            explosionFirstLoop = true;
            explosionBox.SetActive(true);
            this.GetComponent<Renderer>().enabled = false;
        }

        explosionTimer += Time.deltaTime;

        if(explosionTimer >= explosionTime)
        {
            explosionTimer = 0;
            explosionBox.SetActive(false);
            explosionFirstLoop= false;
            respawnRM();
        }
    }

    void respawnRM()
    {
        decayTimer = 0;
        plucked = false;
        currState = Radioactive_State.GROUND;
        this.gameObject.transform.position = originPos;
        this.GetComponent<Renderer>().enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Hazard")
        {
            currState = Radioactive_State.EXPLODING;
        }
    }
}
