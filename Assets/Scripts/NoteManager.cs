using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static NoteManager;

public class NoteManager : MonoBehaviour
{
    public static NoteManager noteManager;

    public GameObject NoteCanvas;
    //list of notes that can be added
    public List<Note_Text_Display> noteList;

    public string noteTitle, noteBody;
    public TextMeshProUGUI noteTitleDisplay, noteBodyDisplay;

    public int displayNoteNum = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if(noteManager != null)
        {
            Destroy(this);
        }
        else
        {
            noteManager = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown("Note"))
        {
            if(noteList != null && NoteCanvas.active == false)
            {
                NoteCanvas.SetActive(true);
                noteList[displayNoteNum].noteDisplayed = true;
                Player_Movement.playerState = Player_Movement.MovementStates.ACTING;
            }
            else
            {
                NoteCanvas.SetActive(false);
                Player_Movement.playerState = Player_Movement.MovementStates.STANDING;
            }
        }

        if (NoteCanvas.active == true && noteList != null)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetButtonDown("Next"))
            {
                nextNote();
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetButtonDown("Previous"))
            {
                nextNote();
            }
        }
    }

    public void nextNote()
    {
        if(displayNoteNum + 1 > noteList.Count - 1)
        {
            displayNoteNum = 0;
        }
        else
        {
            displayNoteNum++;
        }
        if(noteList != null)
        {
            settingText();
        }
    }

    public void previousNote()
    {
        if (displayNoteNum - 1 < 0)
        {
            displayNoteNum = noteList.Count - 1;
        }
        else
        {
            displayNoteNum--;
        }
        if (noteList != null)
        {
            settingText();
        }
    }

    //sets the texts of the notes 
    void settingText()
    {
        noteTitleDisplay.text = noteList[displayNoteNum].noteTitle;
        noteBodyDisplay.text = noteList[displayNoteNum].noteBody;
    }
}
