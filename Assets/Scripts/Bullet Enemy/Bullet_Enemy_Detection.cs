using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Enemy_Detection : MonoBehaviour
{

    public List<string> validTargets;
    public List<GameObject> inRadius;
    public Bullet_Enemy parentScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inRadius.Count != 0)
        {
            parentScript.inRange = true;
        } else
        {
            parentScript.inRange = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (validTargets.Contains(collision.name))
        {
            inRadius.Add(collision.gameObject);
            TargetFinder();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (inRadius.Contains(collision.gameObject))
        {
            inRadius.Remove(collision.gameObject);
            TargetFinder();
        }
    }

    void TargetFinder()
    {
        GameObject target = null;
        int highPri = 0;

        foreach(GameObject g in inRadius)
        {
            if(g.GetComponent<PriorityContainer>().priority >= highPri)
            {
                highPri = g.GetComponent<PriorityContainer>().priority;
                target = g;
            }
        }
        parentScript.enemyInRange = target;
    }
}
