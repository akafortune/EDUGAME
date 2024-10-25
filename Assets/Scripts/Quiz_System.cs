using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiz_System : MonoBehaviour
{
    [Header("Answer Tile Injection")]
    public List<GameObject> answerTiles;

    [Header("Questions and Answers")]
    public List<List<string>> possibleAnswers = new List<List<string>>();
    public List<string> q1Answers, q2Answers, q3Answers;
    public List<string> correctAnswers;
    public string playerAnswer;

    [Header("Timer Values")]
    public float answerTime;
    private float answerTimer;

    private int numberCorrect, questionIndex = 0;

    public bool roundOn = false;
    // Start is called before the first frame update
    void Start()
    {
        possibleAnswers.Add(q1Answers);
        possibleAnswers.Add(q2Answers);
        possibleAnswers.Add(q3Answers);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (roundOn)
        {
            Round();
        }
    }

    public void PrepareField()
    {

        for (int i = 0; i < possibleAnswers[questionIndex].Count; i++)
        {
            answerTiles[i].GetComponent<Answer_Tile>().currAnswer = possibleAnswers[questionIndex][i];
        }

        foreach (GameObject g in answerTiles)
        {
            if (g.GetComponent<Answer_Tile>().currAnswer == "")
            {
                g.SetActive(false);
            }
        }

        roundOn = true;
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

        if (playerAnswer == correctAnswers[questionIndex])
        {
            numberCorrect++;
        }

        playerAnswer = "";
        questionIndex++;

        Debug.Log(numberCorrect);

        if(questionIndex < possibleAnswers.Count)
        {
            PrepareField();
        }
    }
}


