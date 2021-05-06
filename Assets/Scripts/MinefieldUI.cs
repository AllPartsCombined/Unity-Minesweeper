using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinefieldUI : MonoBehaviour
{
    public static MinefieldUI Instance;

    public GameObject mineButtonPrefab;
    public RectTransform minefieldParent;

    private List<Mine> mines = new List<Mine>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    public void InitializeMinefield()
    {
        //Clear minefield
        foreach (var mine in mines)
        {
            Destroy(mine.gameObject);
        }

        mines.Clear();
        GridLayoutGroup grid = minefieldParent.GetComponent<GridLayoutGroup>();
        minefieldParent.sizeDelta = new Vector2(GameController.numCols * grid.cellSize.x + grid.padding.left + grid.padding.right, 
            GameController.numRows * grid.cellSize.y + grid.padding.top + grid.padding.bottom);

        for (int i = 0; i < GameController.numRows * GameController.numCols; i++)
        {
            GameObject mineGO = Instantiate(mineButtonPrefab, minefieldParent);
            mines.Add(mineGO.GetComponent<Mine>());
        }
    }

    public void CheckMine(int ind)
    {

    }
}
