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
    public SpriteRenderer spriteRender;

    public Sprite[] buttonSprite;

    [Header("Timer Values")]
    public static float autosubmitTime = 5;
    public bool runAutoSubmit = false;
    private float autosubmitTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        answerDisplay = this.gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        answerDisplay.text = currAnswer;


        if (runAutoSubmit && GetComponentInParent<Quiz_System>().roundOn)
        {
            autosubmitTimer += Time.deltaTime;

            if (autosubmitTimer >= autosubmitTime)
            {
                autosubmitTimer = 0;
                GetComponentInParent<Quiz_System>().TeardownField();
            }
        } else
        {
            autosubmitTimer = 0;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerOn = true;
            runAutoSubmit = true;
            spriteRender.sprite = buttonSprite[1];
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerOn = false;
            runAutoSubmit = false;
            spriteRender.sprite = buttonSprite[0];
        }
    }
}
