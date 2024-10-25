using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Quiz_System : MonoBehaviour
{
    [Header("Answer Tile Injection")]
    public List<GameObject> answerTiles;

    [Header("Dialogue Box Info")]
    public GameObject dialogueBox;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI questionText;
    private int dialogueIndex = 0;
    private bool inDialogue = false;

    [Header("Questions and Answers")]
    public QuestionAtlas[] quizSheet;
    public string playerAnswer;

    [Header("Fade Out Injection")]
    public SpriteRenderer fader;

    [Header("Timer Values")]
    public float answerTime, fadeOutTime;
    private float answerTimer, fadeOutTimer;

    private int numberCorrect, questionIndex = 0;

    public bool roundOn = false;
    private bool previousCorrect, fadeOut, lastLines = false;
    private List<string> dialogue = new List<string>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (roundOn)
        {
            Round();
        }

        if (inDialogue)
        {
            DialogueController();
        }

        if (fadeOut)
        {
            FadeOut();
        }
    }

    void FadeOut()
    {
        fadeOutTimer += Time.deltaTime;

        fader.color = new Color(0, 0, 0, fadeOutTimer/fadeOutTime);

        if(fadeOutTimer >= fadeOutTime)
        {
            Application.Quit();
        }
    }

    void DialogueController()
    {
        if (Input.GetButtonDown("Stun"))
        {
            if(dialogueIndex < dialogue.Count - 1)
            {
                dialogueIndex++;
            } else
            {
                inDialogue = false;
                roundOn = true;
                dialogueBox.SetActive(false);
                Player_Movement.playerState = Player_Movement.MovementStates.STANDING;

                if (lastLines)
                {
                    fadeOut = true;
                }
            }
        }

        questionText.text = dialogue[dialogueIndex];
    }

    void SetDialogue()
    {
        dialogueIndex = 0;
        dialogue.Clear();

        Player_Movement.playerState = Player_Movement.MovementStates.ACTING;

        if (previousCorrect)
        {
            dialogue.Add("That's Correct!");
            dialogue.Add("Now, on to the next question");
        }
        else if(questionIndex == 0)
        {
            dialogue.Add("Let's begin! Your first question is...");
        }
        else
        {
            dialogue.Add("Incorrect...");
            dialogue.Add("Now, on to the next question");
        }

        
        dialogue.Add(quizSheet[questionIndex].question);

        dialogueBox.SetActive(true);
        inDialogue = true;
    }

    void SetFinalDialogue()
    {
        dialogueIndex = 0;
        dialogue.Clear();

        Player_Movement.playerState = Player_Movement.MovementStates.ACTING;

        if (previousCorrect)
        {
            dialogue.Add("That's Correct!");
        }
        else
        {
            dialogue.Add("Incorrect...");
        }

        dialogue.Add("The exam is now over, good job");
        dialogue.Add("Your final score was " + numberCorrect.ToString() + " out of " + quizSheet.Length.ToString());

        if(numberCorrect == 0 || numberCorrect == 1)
        {
            dialogue.Add("That's a pretty rough score, come back and try again sometime!");
        } else if(numberCorrect == quizSheet.Length || numberCorrect == quizSheet.Length - 1)
        {
            dialogue.Add("Wonderful! It seems you've passed with flying colors");
        } else
        {
            dialogue.Add("Pretty good, but you can always do better!");
        }

        dialogue.Add("Alright, now sit tight and wait for extraction");

        dialogueBox.SetActive(true);
        inDialogue = true;
        lastLines = true;
    }

    public void PrepareField()
    {
        for (int i = 0; i < quizSheet[questionIndex].possibleAnswers.Length; i++)
        {
            answerTiles[i].GetComponent<Answer_Tile>().currAnswer = quizSheet[questionIndex].possibleAnswers[i];
        }

        foreach (GameObject g in answerTiles)
        {
            if (g.GetComponent<Answer_Tile>().currAnswer == "")
            {
                g.SetActive(false);
            }
        }

        SetDialogue();
    }

    void Round()
    {
        answerTimer += Time.deltaTime;

        if (answerTimer >= answerTime)
        {
            TeardownField();
        }
    }

    public void TeardownField()
    {
        roundOn = false;
        answerTimer = 0;

        foreach (GameObject g in answerTiles)
        {
            Answer_Tile tile = g.GetComponent<Answer_Tile>();

            if (tile.playerOn)
            {
                playerAnswer = tile.currAnswer;
            }

            tile.currAnswer = "";
        }

        if (playerAnswer == quizSheet[questionIndex].correctAnswer)
        {
            numberCorrect++;
            previousCorrect = true;
        } else
        {
            previousCorrect = false;
        }

        //playerAnswer = "";
        questionIndex++;

        Debug.Log(numberCorrect);

        if(questionIndex < quizSheet.Length)
        {
            PrepareField();
        } else
        {
            SetFinalDialogue();
        }
    }
}

[System.Serializable]
public class QuestionAtlas{
    public string question;
    public string correctAnswer;
    public string[] possibleAnswers;
}


