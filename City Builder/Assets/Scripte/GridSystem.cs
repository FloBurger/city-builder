using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    private int width;
    private int height;
    private float cellSize;
    private int[,] gridArray;
    private Vector3 origin;

    public GridSystem(int width, int height, float cellSize, Vector3 origin){
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;

        gridArray = new int[width, height];
        for(int x = 0; x < gridArray.GetLength(0); x++){
            for(int z = 0; z < gridArray.GetLength(1); z++){
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z +1), Color.white, 10f);
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z ), Color.white, 10f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 10f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 10f);

    }

    private Vector3 GetWorldPosition(int x, int z){
        int y = 0;
        Vector3 vec = new Vector3(x,y,z);
        return  vec * cellSize + origin;
    }

    public Vector3 GetGridPosition(Vector3 vecPos){
    
        Vector3 vec = vecPos;
        Vector2Int gridTest = GetXY(vec);
        //Debug.Log(gridTest);
        vec = new Vector3(gridTest.x, vecPos.y, gridTest.y);
        return  vec * cellSize + origin;
    }

    private Vector2Int GetXY(Vector3 worldPosition){
        int x = Mathf.FloorToInt((worldPosition - origin).x / cellSize);
        int z = Mathf.FloorToInt((worldPosition - origin).z / cellSize);

        return new Vector2Int(x, z);
    }

    public void SetValue(int x, int z, int value){
        if(x >= 0 && z >= 0 && x < width && z < height)
            gridArray[x,z] = value;
        //Debug.Log(x + "," + z + "," + value);
    }

    public void SetValue(Vector3 worldPosition, int value){
        Vector2Int worldPos = GetXY(worldPosition);
        SetValue(worldPos.x, worldPos.y, value);
    }

    public int getValue(int x, int z){
        if(x >= 0 && z >= 0 && x < width && z < height)
        {
            return gridArray[x,z];
        }
        else{
            return -1;
        }
    }

    public int getValue(Vector3 worldPosition){
        Vector2Int pos = GetXY(worldPosition);
        return getValue(pos.x, pos.y);
    }


}
