using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float timeTillRespawn;
    public float countDown;
    public bool hasDied;

    // Start is called before the first frame update
    void Start()
    {
        //maxHealth = 5;
        health = maxHealth;
        hasDied = false;
        if(timeTillRespawn == 0) { timeTillRespawn = 2.5f; }
        countDown = timeTillRespawn;
    }

    // Update is called once per frame
    void Update()
    {
        // for the player object
        if(gameObject.name.Equals("Player") && health <= 0 && !hasDied)
        {
            print(gameObject.name + " has perished");
            KillPlayer();
        }
        if(hasDied && this.gameObject.name.Equals("Player"))
        {
            // starts the respawn timer
            countDown -= Time.deltaTime;
            print((int)countDown);
            if(countDown <= 0)
            {
                RespawnPlayer(); // once the timer has ended
            }
        }
    }

    public float GetHealth()
    {
        return health;
    }

    public void OnHit(float healthLost)
    {
        health -= healthLost;
        print(gameObject.name + " has lost " + healthLost + " health");
    }

    public void OnHeal(float healthGained)
    {
        health += healthGained;
        print(gameObject.name + " has gained " + healthGained + " health");
    }

    // deletes the player and starts a timer to trigger respawn
    public void KillPlayer()
    {
        // player will go temporarily invisible
        if (this.name.Equals("Player"))
        {
            this.GetComponent<SpriteRenderer>().enabled = false;
            hasDied = true;
        }
        //enemies will dies
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    // respawns the player at the correct respawn point
    public void RespawnPlayer()
    {
        //this.transform.position = door respawn position
        OnHeal((int)(maxHealth/2) + 1); // healing up to half of the health
        hasDied = false;
        countDown = timeTillRespawn;
        this.GetComponent<SpriteRenderer>().enabled = true;
    }

}
