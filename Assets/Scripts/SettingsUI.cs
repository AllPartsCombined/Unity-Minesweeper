using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public Image beginnerCheck, intermediateCheck, expertCheck, customCheck, marksCheck, soundCheck;
    public AudioController audioController;

    // Update is called once per frame
    void Update()
    {
        beginnerCheck.enabled = GameController.Instance.size == GameController.Size.Beginner;
        intermediateCheck.enabled = GameController.Instance.size == GameController.Size.Intermediate;
        expertCheck.enabled = GameController.Instance.size == GameController.Size.Expert;
        customCheck.enabled = GameController.Instance.size == GameController.Size.Custom;
        marksCheck.enabled = GameController.Instance.marksEnabled;
        soundCheck.enabled = audioController.soundOn;
    }
}
