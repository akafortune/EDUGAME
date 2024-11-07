using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class Dialogue_Behavior : MonoBehaviour
{
    public GameObject dialogueObject;
    public TextMeshProUGUI charName, body;
    public TextAsset dialogueFile;
    public char splitChar = '/';
    private int textIndex = 0;
    private string wholeDialogue;
    private string[] brokenDialogue;
    private bool dialogueOpen = false;
    
    // Start is called before the first frame update
    void Start()
    {
        wholeDialogue = dialogueFile.text;
        brokenDialogue = wholeDialogue.Split(splitChar);
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueOpen)
        {
            if(Input.GetButtonDown("Stun"))
            {
                if(textIndex >= brokenDialogue.Length - 1)
                {
                    CloseWindow();
                } else
                {
                    textIndex++;
                    SetBody(textIndex);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player_Movement.playerState = Player_Movement.MovementStates.ACTING;
            OpenWindow();
        }
    }

    private void OpenWindow()
    {
        dialogueObject.SetActive(true);
        dialogueOpen = true;
        charName.text = brokenDialogue[0];
        textIndex = 1;
        SetBody(textIndex);
    }

    private void SetBody(int index)
    {
        body.text = brokenDialogue[index];
    }

    private void CloseWindow()
    {
        charName.text = "";
        body.text = "";
        dialogueObject.SetActive(false);
        Player_Movement.playerState = Player_Movement.MovementStates.STANDING;
        this.gameObject.SetActive(false);
    }
}
