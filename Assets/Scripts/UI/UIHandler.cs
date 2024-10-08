using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Application = UnityEngine.Device.Application;

public class UIHandler : MonoBehaviour
{
    public GameObject SettingsMenu;
    // Start is called before the first frame update
    void Start()
    {
        SettingsMenu.SendMessage("Init");
        ToggleSettings();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettings();
        }
    }

    public void ToggleSettings()
    {
        SettingsMenu.SetActive(!SettingsMenu.activeSelf);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
