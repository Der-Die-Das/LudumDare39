using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{
    public GameObject ScorePrefab;
    public GameObject enterName;
    public GameObject highScores;
    public Text enteredName;
    private GameManager gameManager;
    dreamloLeaderBoard dl;

    void Start()
    {
        gameManager = GameManager.Instance;
        dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
        PlayerPrefs.SetString("Name", "");
        if (PlayerPrefs.GetString("Name", "") == "")
        {
            enterName.SetActive(true);
            highScores.SetActive(false);
        }
        else
        {
            enterName.SetActive(false);
            highScores.SetActive(true);
            OnEnd(PlayerPrefs.GetString("Name", ""));
        }
    }


    public void OnNameEntered()
    {
        if (!string.IsNullOrEmpty(enteredName.text))
        {
            OnEnd(enteredName.text);
        }
    }
    public void OnEnd(string playerName)
    {
        PlayerPrefs.SetString("Name", playerName);
        dl.AddScore(playerName, gameManager.GetPopulation());
        enterName.SetActive(false);
        highScores.SetActive(true);
        StartCoroutine(PopulateHighScores());
    }

    IEnumerator PopulateHighScores()
    {
        List<dreamloLeaderBoard.Score> scores = GetScores();
        while (scores == null || scores.Count == 0)
        {
            yield return 0;
            scores = GetScores();
        }

        foreach (var item in scores)
        {
            GameObject go = Instantiate(ScorePrefab, highScores.transform);
            go.transform.Find("Name").GetComponent<Text>().text = item.playerName;
            go.transform.Find("Score").GetComponent<Text>().text = item.score.ToString();
        }
    }
    public List<dreamloLeaderBoard.Score> GetScores()
    {
        return dl.ToListHighToLow();
    }

}
