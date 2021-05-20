using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomFieldUI : MonoBehaviour
{
    public GameObject panel;
    public InputField heightInput, widthInput, minesInput;
    public Button okButton, cancelButton;

    private void Start()
    {
        Close();
        okButton.onClick.AddListener(() => Close(true));
        cancelButton.onClick.AddListener(() => Close());
    }

    public void Open()
    {
        panel.SetActive(true);
        heightInput.text = GameController.Instance.numRows.ToString();
        widthInput.text = GameController.Instance.numCols.ToString();
        minesInput.text = GameController.Instance.numMines.ToString();
    }

    public void Close(bool save = false)
    {
        if (save)
        {
            int h;
            int w;
            int m;
            GameController.Instance.size = GameController.Size.Custom;
            int def = GameController.Instance.numRows;
            int output;
            if (int.TryParse(heightInput.text, out output))
                h = output;
            else
                h = def;
            def = GameController.Instance.numCols;
            if (int.TryParse(widthInput.text, out output))
                w = output;
            else
                w = def;
            def = GameController.Instance.numMines;
            if (int.TryParse(minesInput.text, out output))
                m = output;
            else
                m = def;
            h = Mathf.Clamp(h, 9, 24);
            w = Mathf.Clamp(w, 9, 30);
            m = Mathf.Clamp(m, 10, 667);
            GameController.Instance.SetSize(w, h, m);
        }
        panel.SetActive(false);
    }

}
