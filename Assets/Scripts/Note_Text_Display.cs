using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Note_Text_Display : MonoBehaviour
{
    public string noteTitle, noteBody;
    public TextMeshProUGUI noteTitleDisplay, noteBodyDisplay;
    public GameObject noteCanvas;

    public bool noteDisplayed = false, readingNote = false;
    // Start is called before the first frame update
    
    void Update()
    {
        if (noteDisplayed)
        {
            NoteInit();
        }

        if (readingNote)
        {
            if (Input.GetButtonDown("Stun"))
            {
                NoteDestroy();
            }
        }
    }

    void NoteDestroy()
    {
        Player_Movement.playerState = Player_Movement.MovementStates.STANDING;
        noteCanvas.SetActive(false);
        readingNote = false;
        this.gameObject.SetActive(false);
    }

    void NoteInit()
    {
        noteTitleDisplay.text = noteTitle;
        noteBodyDisplay.text = noteBody;
        Player_Movement.playerState = Player_Movement.MovementStates.ACTING;
        noteCanvas.SetActive(true);
        readingNote = true;
        noteDisplayed = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        NoteManager.noteManager.noteList.Add(this);
        noteDisplayed = true;
    }
}
