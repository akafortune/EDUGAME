using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class DialougeSystem : MonoBehaviour
{
    //Text Panel
    public TextMeshProUGUI textBox;
    public TextMeshProUGUI nameBox;

    //Text Variables
    GameObject player;
    Collider2D playerCol;
    private List<string>[] rooms = new List<string>[5];
    private float textSpeed = 0.006f;
    private int index;
    private int roomNum;
    private static string text = "";

    // Start is called before the first frame update
    void Start()
    {
        //gameObject.SetActive(false);
        player = GameObject.Find("Player");
        playerCol = playerCol.GetComponent<BoxCollider2D>();
        textBox.text = string.Empty;
        ReadTextFile();
        index = 0;

        //StartCoroutine(DisplayText(0));
    }

    // Update is called once per frame
    void Update()
    {
        //Checks tile for dialouge trigger based on room
        //if(playerCol.OverlapCollider)
        {
            //Player_Movement.MovementStates.ACTING;
            gameObject.SetActive(true);
            //Pop open Window animation
            roomNum = 1;
            StartCoroutine(DisplayText(roomNum));
        }

        //Mouse/Space Bar Click for Dialouge Progression
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && gameObject.activeSelf)
        {
            if (textBox.text == text)
            {
                //plays soundbyte
                NextLine(roomNum);
            }

            else
            {
                textBox.text = text;
            }
        }
    }

    //Takes dialouge from text file and pushes into list
    void ReadTextFile()
    {
        StreamReader sr;
        string line;
        int i = 0;

        do
        {
            sr = new StreamReader(Application.dataPath + "/DialougeAssets/" + "Dialouge" + (i + 1) + ".txt");

            rooms[i] = new List<string>();

            while ((line = sr.ReadLine()) != null)
            {
                rooms[i].Add(line);
            }

            i++;
        }
        while (i < 5);

        sr.Close();
    }

    //Types each character out from each line
    IEnumerator DisplayText(int i)
    {
        text = DisplayPortrait(i);

        foreach (char c in text)
        {
            textBox.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    //Displays portait based on who is speaking, and splits line for other methods line references
    string  DisplayPortrait(int i)
    {
        string line = rooms[i][index];
        string[] name = line.Split("] ");

        //Debug.Log(name[0]);

        switch (name[0])
        {
            case "[Doctor":
                //Display portrait
                nameBox.text = "Doctor";
                break;

            case "[Assistant":
                //Display portrait
                nameBox.text = "Assistant";
                break;

            case "[Warrior":
                //Display portrait
                nameBox.text = "Immuno Warrior";
                break;

            //any other characters, add here
        }

        return (name[1]);
    }

    //Proceeds to next line
    void NextLine(int i)
    {
        if (index < rooms[i].Count - 1)
        {
            index++;
            textBox.text = string.Empty;

            StartCoroutine(DisplayText(i));
        }

        else
        {
            //Close Animation
            gameObject.SetActive(false);
        }
    }


}
