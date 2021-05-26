using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Space : MonoBehaviour
{
    public enum SpaceMode { Active, Flagged, Question, Checked, Exploded, Revealed, Dead }
    public int index;
    public bool mine = false;

    private SpaceMode mode;

    public MultifunctionButton leftClickButton;
    public MultifunctionButton rightClickButton;
    public Image checkedImage;
    public Image flag;
    public Image mineImage;
    public Image xImage;
    public Image explodedImage;
    public Text numberText;
    public Text questionMarkText;
    public NumberStyleMapping numberStyleMapping;

    public SpaceMode Mode
    {
        get => mode;
    }

    private void Awake()
    {
        SetMode(SpaceMode.Active);
    }

    public void SetMode(SpaceMode newMode)
    {
        switch (newMode)
        {
            case SpaceMode.Active:
                flag.gameObject.SetActive(false);
                questionMarkText.gameObject.SetActive(false);
                leftClickButton.interactable = true;
                mineImage.gameObject.SetActive(false);
                xImage.gameObject.SetActive(false);
                explodedImage.gameObject.SetActive(false);
                break;
            case SpaceMode.Flagged:
                flag.gameObject.SetActive(true);
                questionMarkText.gameObject.SetActive(false);
                leftClickButton.interactable = false;
                mineImage.gameObject.SetActive(false);
                break;
            case SpaceMode.Question:
                flag.gameObject.SetActive(false);
                questionMarkText.gameObject.SetActive(true);
                leftClickButton.interactable = true;
                mineImage.gameObject.SetActive(false);
                break;
            case SpaceMode.Checked:
                checkedImage.gameObject.SetActive(true);
                leftClickButton.gameObject.SetActive(false);
                questionMarkText.gameObject.SetActive(false);
                flag.gameObject.SetActive(false);
                mineImage.gameObject.SetActive(false);
                break;
            case SpaceMode.Exploded:
                leftClickButton.gameObject.SetActive(false);
                questionMarkText.gameObject.SetActive(false);
                flag.gameObject.SetActive(false);
                explodedImage.gameObject.SetActive(true);
                mineImage.gameObject.SetActive(true);
                break;
            case SpaceMode.Revealed:
                questionMarkText.gameObject.SetActive(false);
                if (mode != SpaceMode.Flagged)
                {
                    checkedImage.gameObject.SetActive(true);
                    leftClickButton.gameObject.SetActive(false);
                    flag.gameObject.SetActive(false);
                    mineImage.gameObject.SetActive(true);
                }
                break;
            case SpaceMode.Dead:
                leftClickButton.interactable = false;
                rightClickButton.interactable = false;
                if (mode == SpaceMode.Flagged)
                {
                    flag.gameObject.SetActive(false);
                    checkedImage.gameObject.SetActive(true);
                    leftClickButton.gameObject.SetActive(false);
                    xImage.gameObject.SetActive(true);
                    mineImage.gameObject.SetActive(true);
                }
                break;
        }
        mode = newMode;
    }

    public void HandleRightClick()
    {
        switch (mode)
        {
            case SpaceMode.Active:
                SetMode(SpaceMode.Flagged);
                break;
            case SpaceMode.Flagged:
                if(GameController.Instance.marksEnabled)
                    SetMode(SpaceMode.Question);
                else
                    SetMode(SpaceMode.Active);
                break;
            case SpaceMode.Question:
                SetMode(SpaceMode.Active);
                break;
        }
    }

    public void SetNumber(int number)
    {
        numberText.gameObject.SetActive(number != 0);
        numberText.text = number.ToString();
        var styleElement = numberText.GetComponent<StyleElementText>();
        if (styleElement != null)
        {
            styleElement.id = numberStyleMapping.GetStyle(number);
            styleElement.Apply();
        }
        SetMode(SpaceMode.Checked);
    }

    public void Init()
    {
        leftClickButton.OnClickUp.AddListener(() => GameController.Instance.HandleClick(index));
    }
}

[System.Serializable]
public class NumberStyleMapping
{
    public string oneStyle = "Number1";
    public string twoStyle = "Number2";
    public string threeStyle = "Number3";
    public string fourStyle = "Number4";
    public string fiveStyle = "Number5";
    public string sixStyle = "Number6";
    public string sevenStyle = "Number7";
    public string eightStyle = "Number8";

    public string GetStyle(int number)
    {
        switch (number)
        {
            case 1:
                return oneStyle;
            case 2:
                return twoStyle;
            case 3:
                return threeStyle;
            case 4:
                return fourStyle;
            case 5:
                return fiveStyle;
            case 6:
                return sixStyle;
            case 7:
                return sevenStyle;
            case 8:
                return eightStyle;
        }
        return "Number1";
    }
}