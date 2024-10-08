using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject roomCam;

    // Start is called before the first frame update
    void Start()
    {
        //roomCam = GetComponentInChildren<GameObject>(true);
        roomCam = gameObject.transform.GetChild(0).gameObject;
        //print(GetComponentInChildren<GameObject>(true).name);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // sets a room camera to true when the room is entered
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag.Equals("Player") && !other.isTrigger)
        {
            roomCam.SetActive(true);
        }
    }

    // sets a room camera to false when the room is exited
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player") && !other.isTrigger)
        {
            roomCam.SetActive(false);
        }
    }
}
