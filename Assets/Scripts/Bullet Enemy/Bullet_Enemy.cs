using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Enemy : MonoBehaviour
{
    public bool inRange = false, stunned = true;
    public GameObject bullet, enemyInRange = null;
    public Transform spawnPos;
    public Animator anim;

    public float fireTime, stunTime;
    private float fireTimer = 0, stunTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!stunned)
        {
            if (!inRange)
            {
                //reset shoot timer
                fireTimer = 0;
            }

            if (inRange)
            {
                transform.right = enemyInRange.transform.position - transform.position;
                spawnPos.rotation = this.gameObject.transform.rotation;

                //increment shoot timer, shoot if over
                ShootClock();
            }
        } else
        {
            //count down stun clock
            StunClock();
        }
        
    }

    void StunClock()
    {
        stunTimer += Time.deltaTime;

        if(stunTimer >= stunTime)
        {
            stunned = false;
            stunTimer = 0;
        }
    }

    void ShootClock()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireTime)
        {
            //need to calculate direction of player from the enemy to determine which direction to use for animation
            //triggers attack animation
            anim.SetTrigger("Attack Trigger");
            anim.SetFloat("moveX", 1);
            Instantiate(bullet, spawnPos.position, this.transform.rotation);
            fireTimer = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Stun")
        {
            stunned = true;
        }
    }
}
