using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    Image[] gObjects; // holds all of the game objects including the parent
    public Image[] batteryCells; // will only have the battery cell images
    public HealthSystem pHealthSystem;

    // Start is called before the first frame update
    void Start()
    {

        gObjects = GetComponentsInChildren<Image>();
        batteryCells = new Image[gObjects.Length - 1];

        // grabs the just the power cell images
        int j = 0;
        for(int i = 0; i < gObjects.Length;i++)
        {
            if (gObjects[i].name.Contains("Cell"))
            {
                batteryCells[j] = gObjects[i];
                j++;
            }
        }
        
        if(GameObject.Find("Player") != null)
        {
            pHealthSystem = GameObject.Find("Player").GetComponent<HealthSystem>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerHealth(pHealthSystem.GetHealth());
    }

    public void CheckPlayerHealth(float health)
    {
        //if(pHealthSystem.GetHealth() == 3)
        //{
        //    for(int i = 0; i < batteryCells.Length;i++)
        //    {
        //        batteryCells[i].enabled = true;
        //    }
        //}
        //else if(pHealthSystem.GetHealth() == 3)
        //{

        //}

        for (int i = 0; i < batteryCells.Length; i++)
        {
            if(i <= health - 1)
            {
                batteryCells[i].enabled = true;
            }
            else
            {
                batteryCells[i].enabled = false;
            }
        }
    }
}
