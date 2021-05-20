using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoresUI : MonoBehaviour
{
    public GameObject panel;
    public HighScoreUI beginner, intermediate, expert;
    public Button resetButton, okButton;

    private void Awake()
    {
        resetButton.onClick.AddListener(() => ResetScores());
        okButton.onClick.AddListener(() => Close());
        Close();
    }

    public void Open()
    {
        panel.SetActive(true);
        var hs = GameController.Instance.GetHighScore(GameController.Size.Beginner);
        beginner.scoreText.text = hs.time.ToString();
        beginner.nameText.text = hs.name;
        hs = GameController.Instance.GetHighScore(GameController.Size.Intermediate);
        intermediate.scoreText.text = hs.time.ToString();
        intermediate.nameText.text = hs.name;
        hs = GameController.Instance.GetHighScore(GameController.Size.Expert);
        expert.scoreText.text = hs.time.ToString();
        expert.nameText.text = hs.name;
    }

    public void Close()
    {
        panel.SetActive(false);
    }

    public void ResetScores()
    {
        PlayerPrefs.DeleteKey(GameController.Instance.GetTimePref(GameController.Size.Beginner));
        PlayerPrefs.DeleteKey(GameController.Instance.GetNamePref(GameController.Size.Beginner));
        PlayerPrefs.DeleteKey(GameController.Instance.GetTimePref(GameController.Size.Intermediate));
        PlayerPrefs.DeleteKey(GameController.Instance.GetNamePref(GameController.Size.Intermediate));
        PlayerPrefs.DeleteKey(GameController.Instance.GetTimePref(GameController.Size.Expert));
        PlayerPrefs.DeleteKey(GameController.Instance.GetNamePref(GameController.Size.Expert));
        Open();
    }
}

[System.Serializable]
public class HighScoreUI
{
    public Text scoreText, nameText;
}
