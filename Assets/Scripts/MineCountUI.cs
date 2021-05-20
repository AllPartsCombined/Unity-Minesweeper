using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineCountUI : MonoBehaviour
{
    public Text countText;

    public int GetCount()
    {
        int count = GameController.Instance.numMines;
        foreach (var space in GameController.Instance.Spaces)
        {
            if (space.Mode == Space.SpaceMode.Flagged)
                count--;
        }
        return count;
    }

    private void Update()
    {
        int count = GetCount();
        countText.text = count < 0 ? count.ToString("00") : count.ToString("000");
    }
}
