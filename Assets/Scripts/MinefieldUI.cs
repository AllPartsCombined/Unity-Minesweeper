using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinefieldUI : MonoBehaviour
{
    public static MinefieldUI Instance;

    public GameObject spacePrefab;
    public RectTransform minefieldParent;

    private List<Space> spaces = new List<Space>();

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
        foreach (var mine in spaces)
        {
            Destroy(mine.gameObject);
        }

        spaces.Clear();
        GridLayoutGroup grid = minefieldParent.GetComponent<GridLayoutGroup>();
        minefieldParent.sizeDelta = new Vector2(GameController.numCols * grid.cellSize.x + grid.padding.left + grid.padding.right,
            GameController.numRows * grid.cellSize.y + grid.padding.top + grid.padding.bottom);

        for (int i = 0; i < GameController.numRows * GameController.numCols; i++)
        {
            GameObject spaceGO = Instantiate(spacePrefab, minefieldParent);
            Space space = spaceGO.GetComponent<Space>();
            space.name = "Space " + i;
            space.index = i;
            space.Init();
            spaces.Add(space);
        }
    }

    public int CheckSpace(int ind)
    {
        if (ind != -1)
        {
            //mines[ind].SetMode(Space.Mode.Checked);
            if (spaces[ind].mine)
                return 1;
        }
        return 0;
    }

    public void HandleClick(int ind)
    {
        Vector2Int rc = GameController.ind2rc(ind);
        Debug.Log("Clicked Space: " + ind + ". (" + rc.x + ", " + rc.y + ").");
        // Initialize the field if it's the first time:
        if (!GameController.gameStarted)
        {
            GameController.StartGame();
            var randomMineIndices = GameController.GetRandomMineIndices(ind);
            foreach (int index in randomMineIndices)
            {
                spaces[index].mine = true;
            }
            //field(ind) = 1; // First hit cannot lose
            //[sorted, indices] = sort(field);
            //mineIndices = indices(1:numMines);
            //started = true;
            //start(h.timerObject);
        }

        // Check for mine:
        if (CheckSpace(ind) == 0)
        {
            SurroundCount(ind); // Check Surrounding spots
                                //if ishandle(my_h)
                                //    delete(my_h); % Delete this button if it hasn't been
        }
        else
        {
            Lose(ind);
        }

        //if length(goodInds) == totalGood
        //    Win;
        //    end
    }

    private void Lose(int explodedIndex)
    {
        foreach (var space in spaces)
        {
            if (space.mine)
            {
                if (space.index == explodedIndex)
                    space.SetMode(Space.SpaceMode.Exploded);
                else
                    space.SetMode(Space.SpaceMode.Revealed);
            }
            else
                space.SetMode(Space.SpaceMode.Dead);
        }
    }

    public void SurroundCount(int ind)
    {

        if (ind != -1 && (spaces[ind].Mode == Space.SpaceMode.Active || spaces[ind].Mode == Space.SpaceMode.Question))
        {
            Vector2Int rc = GameController.ind2rc(ind);
            int bottomLeft = GameController.rc2ind(rc.x + 1, rc.y - 1);
            int middleLeft = GameController.rc2ind(rc.x, rc.y - 1);
            int topLeft = GameController.rc2ind(rc.x - 1, rc.y - 1);
            int bottomMiddle = GameController.rc2ind(rc.x + 1, rc.y);
            int topMiddle = GameController.rc2ind(rc.x - 1, rc.y);
            int bottomRight = GameController.rc2ind(rc.x + 1, rc.y + 1);
            int middleRight = GameController.rc2ind(rc.x, rc.y + 1);
            int topRight = GameController.rc2ind(rc.x - 1, rc.y + 1);
            int count = 0;
            count += CheckSpace(bottomLeft);
            count += CheckSpace(middleLeft);
            count += CheckSpace(topLeft);
            count += CheckSpace(bottomMiddle);
            count += CheckSpace(topMiddle);
            count += CheckSpace(bottomRight);
            count += CheckSpace(middleRight);
            count += CheckSpace(topRight);
            if (count == 0) //No mines around, clear the spot and do same for surrounding spots.
            {
                spaces[ind].SetMode(Space.SpaceMode.Checked);
                SurroundCount(bottomLeft);
                SurroundCount(middleLeft);
                SurroundCount(topLeft);
                SurroundCount(bottomMiddle);
                SurroundCount(topMiddle);
                SurroundCount(bottomRight);
                SurroundCount(middleRight);
                SurroundCount(topRight);
            }
            else
            {
                spaces[ind].SetNumber(count);
                //DrawNewSpot(ind, num2str(count), GetColor(count));
            }
        }
    }

}
