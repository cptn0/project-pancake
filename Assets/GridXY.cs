using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class GridXY {

    private int width;
    private int height;
    private Vector3 cellSize;
    private Vector3 originPosition;
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;

    public GridXY(int width, int height, Vector3 cellSize, Vector3 originPosition) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++) {
            for (int y = 0; y < gridArray.GetLength(1); y++) {
                debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize.x, cellSize.y) * .5f, 20, Color.white, TextAnchor.MiddleCenter);   //5.50
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    private Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x * cellSize.x, y * cellSize.y) + originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y) {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize.x);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize.y);
    }

    public void SetValue(int x, int y, int value) {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }

    public void SetValue(Vector3 worldPosition, int value) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    public int GetValue(int x, int y) {
        if (x >= 0 && y >= 0 && x < width && y < height) {
            return gridArray[x, y];
        }
        else return 0;
    }
    public int GetValue(Vector3 worldPosition) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    public void MirrorGrid(out GridXY gridII, out GridXY gridIII, out GridXY gridIV) {
        gridII = new GridXY(width, height, new Vector3(-cellSize.x, cellSize.y), originPosition);
        gridIII = new GridXY(width, height, new Vector3(-cellSize.x, -cellSize.y), originPosition);
        gridIV = new GridXY(width, height, new Vector3(cellSize.x, -cellSize.y), originPosition);
    }

    public void MirrorGridValue(GridXY gridII, GridXY gridIII, GridXY gridIV) {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                gridII.SetValue(x, y, GetValue(x, y));
                gridIII.SetValue(x, y, GetValue(x, y));
                gridIV.SetValue(x, y, GetValue(x, y));
            }
        }
    }
}
