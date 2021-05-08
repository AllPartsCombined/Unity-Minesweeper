using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static int numCols;
    public static int numRows;
    public static int numMines;
    public static bool gameStarted;

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
        MinefieldUI.Instance.InitializeMinefield();
    }

    public static void StartGame()
    {
        gameStarted = true;
        Timer.Instance.StartTimer();
    }

    //Convert from an index to row, col format
    public static Vector2Int ind2rc(int ind)
    {
        return new Vector2Int(Mathf.FloorToInt(ind / numCols), ind % numCols);
    }

    //Convert from a row, col pair to index format
    public static int rc2ind(int row, int col)
    {
        if (row < 0 || col < 0 || row >= numRows || col >= numCols)
            return -1;
        return (row) * numCols + col;
    }

    public static List<int> GetRandomMineIndices(int excluding = -1)
    {
        List<int> l = new List<int>();
        for (int i = 0; i < numMines; i++)
        {
            int randomNumber = -1;
            while (randomNumber < 0 || l.Contains(randomNumber) || randomNumber == excluding)
            {
                randomNumber = Random.Range(0, numCols * numRows);
            }
            l.Add(randomNumber);
        }
        return l;
    }
}