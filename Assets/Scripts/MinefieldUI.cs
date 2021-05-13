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

    private int clearedSpaces = 0;

    private List<Space> spaces = new List<Space>();

    public event System.Action OnWin;
    public event System.Action OnLose;
    public event System.Action OnInit;

    public List<Space> Spaces
    {
        get => spaces;
    }

    public int numCols;
    public int numRows;
    public int numMines;
    public bool gameStarted;

    public int WinCount
    {
        get => (numCols * numRows) - numMines;
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        gameStarted = false;
        numCols = 30;
        numRows = 16;
        numMines = 50;
        InitializeMinefield();
        Timer.Instance.StopTimer(true);
        if (OnInit != null)
            OnInit();
    }

    public void StartGame()
    {
        gameStarted = true;
        Timer.Instance.StartTimer();
    }

    //Convert from an index to row, col format
    public Vector2Int ind2rc(int ind)
    {
        return new Vector2Int(Mathf.FloorToInt(ind / numCols), ind % numCols);
    }

    //Convert from a row, col pair to index format
    public int rc2ind(int row, int col)
    {
        if (row < 0 || col < 0 || row >= numRows || col >= numCols)
            return -1;
        return (row) * numCols + col;
    }

    public List<int> GetRandomMineIndices(int excluding = -1)
    {
        List<int> l = new List<int>();
        for (int i = 0; i < numMines; i++)
        {
            int randomNumber = -1;
            while (randomNumber < 0 || l.Contains(randomNumber) || randomNumber == excluding)
            {
                randomNumber = UnityEngine.Random.Range(0, numCols * numRows);
            }
            l.Add(randomNumber);
        }
        return l;
    }

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
        minefieldParent.sizeDelta = new Vector2(numCols * grid.cellSize.x + grid.padding.left + grid.padding.right,
            numRows * grid.cellSize.y + grid.padding.top + grid.padding.bottom);

        for (int i = 0; i < numRows * numCols; i++)
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
        Vector2Int rc = ind2rc(ind);
        Debug.Log("Clicked Space: " + ind + ". (" + rc.x + ", " + rc.y + ").");
        // Initialize the field if it's the first time:
        if (!gameStarted)
        {
            StartGame();
            var randomMineIndices = GetRandomMineIndices(ind);
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


        if (clearedSpaces == WinCount)
        {
            Win();
        }
    }

    private void Lose(int explodedIndex)
    {
        Timer.Instance.StopTimer();
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
        if (OnLose != null)
            OnLose();
    }

    private void Win()
    {
        Timer.Instance.StopTimer();
        foreach (var space in spaces)
        {
            if (space.mine)
            {
                space.SetMode(Space.SpaceMode.Flagged);
            }
        }
        if (OnWin != null)
            OnWin();
    }

    public void SurroundCount(int ind)
    {

        if (ind != -1 && (spaces[ind].Mode == Space.SpaceMode.Active || spaces[ind].Mode == Space.SpaceMode.Question))
        {
            Vector2Int rc = ind2rc(ind);
            int bottomLeft = rc2ind(rc.x + 1, rc.y - 1);
            int middleLeft = rc2ind(rc.x, rc.y - 1);
            int topLeft = rc2ind(rc.x - 1, rc.y - 1);
            int bottomMiddle = rc2ind(rc.x + 1, rc.y);
            int topMiddle = rc2ind(rc.x - 1, rc.y);
            int bottomRight = rc2ind(rc.x + 1, rc.y + 1);
            int middleRight = rc2ind(rc.x, rc.y + 1);
            int topRight = rc2ind(rc.x - 1, rc.y + 1);
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
            clearedSpaces++;
        }
    }

}
