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
    public GameObject player, actBox;
    public Vector3 throwDir, trueScale;
    public BoxCollider2D barrelCollider;
    public float throwTime, throwDist, scaleValue;
    private float throwTimer = 0;
    private bool rise = true;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        trueScale = this.gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    void PickedUp()
    {
        this.gameObject.transform.position = player.GetComponent<Player_Movement>().carryPos.position;
        barrelCollider.isTrigger = true;
    }

    void Ground()
    {

        barrelCollider.isTrigger = false;
    }

    void Thrown()
    {
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
            currState = Radioactive_State.GROUND;
            rise = true;
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, 0);
            this.gameObject.transform.localScale = trueScale;
        }
    }
}
