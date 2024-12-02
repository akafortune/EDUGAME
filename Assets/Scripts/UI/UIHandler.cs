using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Application = UnityEngine.Device.Application;
using UnityEngine.SceneManagement;

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
        if (SettingsMenu.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene("Dungeon 1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
