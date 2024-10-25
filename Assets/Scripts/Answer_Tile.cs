using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Answer_Tile : MonoBehaviour
{
    [Header("Accessed by Quiz System")]
    public string currAnswer;
    public bool playerOn;

    private TextMeshProUGUI answerDisplay;

    [Header("Timer Values")]
    public static float autosubmitTime = 5;
    private float autosubmitTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        answerDisplay = this.gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        answerDisplay.text = currAnswer;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && GetComponentInParent<Quiz_System>().roundOn)
        {
            playerOn = true;
            autosubmitTimer += Time.deltaTime;

            if(autosubmitTimer >= autosubmitTime)
            {
                autosubmitTimer = 0;
                GetComponentInParent<Quiz_System>().TeardownField();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && GetComponentInParent<Quiz_System>().roundOn)
        {
            autosubmitTimer = 0;
            playerOn = false;
        }
    }
}
