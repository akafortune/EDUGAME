using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float health;
    public float maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 5;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            this.gameObject.SetActive(false);
            print(gameObject.name + " has perished");
        }
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
}
