using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewHighScoreUI : MonoBehaviour
{
    public GameObject highScoreUI;
    public Text text;
    public InputField input;
    public Button button;

    private HighScore highScore;

    private void Start()
    {
        GameController.Instance.OnNewHighScore += HandleNewHighScore;
        button.onClick.AddListener(() => Close());
    }

    private void HandleNewHighScore(HighScore hs)
    {
        highScoreUI.SetActive(true);
        text.text = "You have the fastest time for " + hs.size.ToString().ToLower() + " level.\nPlease enter your name.";
        var oldScore = GameController.Instance.GetHighScore(hs.size);
        if (!oldScore.name.Equals("-"))
        {
            input.text = oldScore.name;
        }
        else
        {
            input.text = "";
        }
        highScore = hs;
    }

    public void Close()
    {
        if (highScore != null)
        {
            highScore.name = input.text;
            GameController.Instance.SetHighScore(highScore);
            highScore = null;
        }
        highScoreUI.SetActive(false);
    }
}
