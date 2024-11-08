using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using UnityEngine.EventSystems;

public class ButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button thisButton;

    public Outline wordOutline;

    private AudioSource source;
    public AudioClip pressed;
    // Start is called before the first frame update
    void Start()
    {
        thisButton = this.gameObject.transform.GetComponent<Button>();
        wordOutline = this.gameObject.transform.GetChild(0).GetComponent<Outline>();
        wordOutline.effectColor = new Color32(129, 38, 77, 255);
        source = gameObject.transform.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //thisButton.onClick.AddListener(ButtonPressed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        wordOutline.effectColor = new Color32(6, 167, 150,255);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        wordOutline.effectColor = new Color32(129, 38, 77, 255);
    }

    void ButtonPressed()
    {
        source.PlayOneShot(pressed);
    }
}
