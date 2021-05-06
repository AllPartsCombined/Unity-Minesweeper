using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static int numCols;
    public static int numRows;

    private void Start()
    {
        numCols = 30;
        numRows = 16;
        MinefieldUI.Instance.InitializeMinefield();
    }

    //Convert from an index to row, col format
    public static Vector2Int ind2rc(int ind)
    {
        return new Vector2Int(Mathf.FloorToInt((ind - 1) / numCols), ind - 1 % numCols);
    }

    //Convert from a row, col pair to index format
    public static int rc2ind(int row, int col)
    {
        if (row < 0 || col < 0 || row >= numRows || col >= numCols)
            return -1;
        return (row - 1) * numCols + col;
    }
}