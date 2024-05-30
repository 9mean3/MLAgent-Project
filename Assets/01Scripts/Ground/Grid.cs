using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GroundType
{
    Wood,
    Road,
    //Water,
    EndEnum,
}

public class Grid
{
    public int Width;
    public int Height;
    public int Depth;
    public float CellSize;

    private Ground[,,] grid;

    public Grid(int width, int height, int depth, float cellSize)
    {
        Width = width;
        Height = height;
        Depth = depth;
        CellSize = cellSize;

        grid = new Ground[width, height, depth];
    }

    public void SetBlock(int x, int y, int z, Ground ground)
    {
        if (x >= 0 && y >= 0 && x < Width && y < Height)
        {
            grid[x, y, z] = ground;
        }
    }

    public Ground GetBlock(int x, int y, int z)
    {
        return grid[x, y, z];
    }

    public Vector3 GetWorldPosition(int x, int y, int z)
    {
        return new Vector3(x, y, z) * CellSize;
    }
}
