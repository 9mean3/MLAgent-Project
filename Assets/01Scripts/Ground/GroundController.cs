using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GroundController : MonoBehaviour
{
    public Dictionary<GroundType, Ground> GroundDictionary = new Dictionary<GroundType, Ground>();
    public Grid GridMap;

    [SerializeField] private Ground _checkBlock;
    [SerializeField] private Ground _fenceBlock;
    [SerializeField] private List<Ground> _groundList = new List<Ground>();
    [SerializeField] private int _width;
    //[SerializeField] private int _height;
    [SerializeField] private int _depth;

    private int _loadedLineIdx = 0;

    private void Awake()
    {
        foreach (Ground item in _groundList)
        {
            GroundDictionary.Add(item.GroundType, item);
        }

        GridMap = new Grid(_width, 1, _depth+1, 1, Vector3.zero);


        for (int i = 0; i < _width; i++)
        {
            GridMap.SetBlock(i, 0, 0, _fenceBlock);
            GridMap.SetBlock(i, 0, _depth, _fenceBlock);

            GridMap.SetBlock(i, 0, 1, _checkBlock);
            GridMap.SetBlock(i, 0, _depth - 1, _checkBlock);
        }
        for (int j = 0; j < _depth; j++)
        {
            GridMap.SetBlock(0, 0, j, _fenceBlock);
            GridMap.SetBlock(_width - 1, 0, j, _fenceBlock);
            Debug.Log(_width);
        }

        for (int j = 2; j < _depth; j++)
        {
            SetRandomLine(j);
        }

        GenerateBlock(0, 0, _width, 30);
    }

    private void SetRandomLine(int j)
    {
        GroundType type = (GroundType)Random.Range(0, (int)GroundType.EndEnum);
        for (int i = 1; i < _width - 2; i++)
        {
            GridMap.SetBlock(i, 0, j, GroundDictionary[type]);
        }
    }

    private void GenerateBlock(int startX, int startZ, int endX, int endZ)
    {
        for (int j = startZ; j <= endZ; j++)
        {
            float r = Random.value;
            for (int i = startX; i < endX; i++)
            {
                Vector3 position = GridMap.GetWorldPosition(i, 0, j);
                Ground ground = Instantiate(GridMap.GetBlock(i, 0, j), position, Quaternion.identity);

                ground.SpawnBlock(i, r, this);
            }
        }
    }
}
