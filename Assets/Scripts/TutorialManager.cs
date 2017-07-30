using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public Dialog[] dialogs;
    private int activeDialog;
    public GameObject tutorial;
    public Text text;
    public Text nextText;

    void Start()
    {
        if (PlayerPrefs.GetInt("tutorial", 0) == 0)
        {
            SetDialog(0);
        }
        else
        {
            SetDialog(dialogs.Length);
        }
    }

    public void SetDialog(int index)
    {
        if (index < dialogs.Length)
        {
            activeDialog = index;
            text.text = dialogs[activeDialog].Text;
            nextText.text = dialogs[activeDialog].NextText;
        }
        else
        {
            tutorial.SetActive(false);
            PlayerPrefs.SetInt("tutorial", 1);
        }
    }

    public void NextDialog()
    {
        SetDialog(activeDialog + 1);
    }
}
