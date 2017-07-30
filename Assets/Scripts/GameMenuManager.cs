using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    public GameObject GameMenu;
    public GameObject Options;


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            ChangeGameMenuState();
        }
    }

    public void OpenGameMenu()
    {
        GameMenu.SetActive(true);
    }
    public void ChangeGameMenuState()
    {
        if (GameMenu.activeSelf)
        {
            CloseGameMenu();
        }
        else
        {
            OpenGameMenu();
        }
    }
    public void CloseGameMenu()
    {
        GameMenu.SetActive(false);
        CloseOptions();
    }
    public void OpenOptions()
    {
        Options.SetActive(true);
    }
    public void CloseOptions()
    {
        Options.SetActive(false);
    }
    public void BackToDesktop()
    {
        Application.Quit();
    }

}
