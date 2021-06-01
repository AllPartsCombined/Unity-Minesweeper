using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public enum Size { Beginner, Intermediate, Expert, Custom }
    public static GameController Instance;

    public GameObject spacePrefab;
    public RectTransform minefieldParent;
    public GameObject blocker;

    public int numCols;
    public int numRows;
    public int numMines;

    public Size size;

    [HideInInspector]
    public bool marksEnabled { get; private set; } = true;

    private int clearedSpaces = 0;
    private bool gameStarted;

    private List<Space> spaces = new List<Space>();

    public event System.Action OnWin;
    public event System.Action OnLose;
    public event System.Action OnInit;
    public event System.Action<HighScore> OnNewHighScore;

    public List<Space> Spaces
    {
        get => spaces;
    }

    public int WinCount
    {
        get => (numCols * numRows) - numMines;
    }

    private void Start()
    {
        StartCoroutine(DelayedInitialize());
    }

    IEnumerator DelayedInitialize()
    {
        yield return new WaitForEndOfFrame();
        Initialize();
    }

    public void SetToBeginner()
    {
        SetSize(9, 9, 10);
        size = Size.Beginner;
    }

    public void SetToIntermediate()
    {
        SetSize(16, 16, 40);
        size = Size.Intermediate;
    }

    public void SetToExpert()
    {
        SetSize(30, 16, 99);
        size = Size.Expert;
    }

    public void SetToCustomFromSaved()
    {
        SetSize(PlayerPrefs.GetInt("numCols"), PlayerPrefs.GetInt("numRows"), PlayerPrefs.GetInt("numMines"));
        size = Size.Custom;
    }

    public void SetToCustom()
    {
        size = Size.Custom;
    }

    public void SetSize(int cols, int rows, int mines)
    {
        numCols = cols;
        numRows = rows;
        numMines = mines;
        Initialize();
    }

    public void Initialize()
    {
        gameStarted = false;
        blocker.SetActive(false);
        clearedSpaces = 0;
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
        size = (Size)PlayerPrefs.GetInt("size", 0);
        marksEnabled = PlayerPrefs.GetInt("marks", 1) == 1;
        switch (size)
        {
            case Size.Beginner:
                SetToBeginner();
                break;
            case Size.Intermediate:
                SetToIntermediate();
                break;
            case Size.Expert:
                SetToExpert();
                break;
            case Size.Custom:
                SetToCustomFromSaved();
                break;
        }
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("size", (int)size);
        PlayerPrefs.SetInt("marks", marksEnabled ? 1 : 0);
        if (size == Size.Custom)
        {
            PlayerPrefs.SetInt("numRows", numRows);
            PlayerPrefs.SetInt("numCols", numCols);
            PlayerPrefs.SetInt("numMines", numMines);
        }
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
        blocker.transform.SetAsLastSibling();
    }

    public void ToggleMarksEnabled()
    {
        marksEnabled = !marksEnabled;
    }

    public int CheckSpace(int ind)
    {
        if (ind != -1)
        {
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
        }

        // Check for mine:
        if (CheckSpace(ind) == 0)
        {
            SurroundCount(ind); // Check Surrounding spots

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
        blocker.SetActive(true);
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
        blocker.SetActive(true);
        HighScoreCheck();
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
            }
            clearedSpaces++;
        }
    }

    public HighScore GetHighScore(Size s)
    {
        int t = PlayerPrefs.GetInt(GetTimePref(s), 999);
        string n = PlayerPrefs.GetString(GetNamePref(s), "-");
        return new HighScore(s, t, n);
    }

    public void SetHighScore(Size s, int t, string n)
    {
        PlayerPrefs.SetInt(GetTimePref(s), t);
        PlayerPrefs.SetString(GetNamePref(s), n);
    }

    public void SetHighScore(HighScore hs)
    {
        SetHighScore(hs.size, hs.time, hs.name);
    }

    public void HighScoreCheck()
    {
        if (size == Size.Custom)
            return;
        var hs = GetHighScore(size);
        if (Timer.Instance.Time < hs.time)
        {
            var newHs = new HighScore(size, Timer.Instance.Time, "");
            if (OnNewHighScore != null)
                OnNewHighScore(newHs);
        }
    }

    public string GetTimePref(Size s)
    {
        switch (s)
        {
            case Size.Beginner:
                return "BeginnerTime";
            case Size.Intermediate:
                return "IntermediateTime";
            case Size.Expert:
                return "ExpertTime";
            default:
                return "";
        }
    }

    public string GetNamePref(Size s)
    {
        switch (s)
        {
            case Size.Beginner:
                return "BeginnerName";
            case Size.Intermediate:
                return "IntermediateName";
            case Size.Expert:
                return "ExpertName";
            default:
                return "";
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}

[System.Serializable]
public class HighScore
{
    public GameController.Size size;
    public int time;
    public string name;

    public HighScore(GameController.Size s, int t, string n)
    {
        size = s;
        time = t;
        name = n;
    }
}