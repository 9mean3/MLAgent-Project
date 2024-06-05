using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    public int Width;
    public int Height;
    public int Depth;
    public float CellSize;
    private Vector3 _originPos;

    private Ground[,,] grid;

    public Grid(int width, int height, int depth, float cellSize, Vector3 originPos)
    {
        Width = width;
        Height = height;
        Depth = depth;
        CellSize = cellSize;
        _originPos = originPos;

        grid = new Ground[width, height, depth];
    }

    public void SetBlock(int x, int y, int z, Ground ground)
    {
        if (x >= 0 && y >= 0 && x < Width && y < Height)
        {
            grid[x, y, z] = ground;
        }
        else
        {
            Debug.LogError($"Pos: {x} {y} {z} {ground} is OutOfRange");
        }
    }

    public Ground GetBlock(int x, int y, int z)
    {
        return grid[x, y, z];
    }

    public Vector3 GetWorldPosition(int x, int y, int z)
    {
        return _originPos + new Vector3(x, y, z) * CellSize;
    }

    public Vector3Int WorldToGridPosition(Vector3 worldPosition)
    {
        Vector3 offset = worldPosition - _originPos;
        int x = Mathf.FloorToInt(offset.x / CellSize);
        int y = Mathf.FloorToInt(offset.y / CellSize);
        int z = Mathf.FloorToInt(offset.z / CellSize);

        return new Vector3Int(x, y, z);
    }
}
