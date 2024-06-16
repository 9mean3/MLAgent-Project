using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

[System.Serializable]
public struct GroundSpawnData
{
    public Ground Ground;
    public GroundType GroundType;
    public int Perc;
}

public class GroundController : MonoBehaviour
{
    public Dictionary<GroundType, Ground> GroundDictionary = new Dictionary<GroundType, Ground>();
    public Grid GridMap;

    [SerializeField] private Ground _checkBlock;
    [SerializeField] private Ground _fenceBlock;
    [SerializeField] private List<GroundSpawnData> _groundList = new List<GroundSpawnData>();
    [SerializeField] private int _width;
    //[SerializeField] private int _height;
    [SerializeField] private int _depth;

    public int LoadedLineIdx = 0;

    private void Awake()
    {
/*        foreach (Ground item in _groundList)
        {
            GroundDictionary.Add(item.GroundType, item);
        }*/
        for (int i = 0; i < _groundList.Count; i++)
        {
            Ground item = _groundList[i].Ground;
            GroundDictionary.Add(item.GroundType, item);
        }

        GridMap = new Grid(_width, 1, _depth, 1, Vector3.zero);


        for (int i = 0; i < _width; i++)
        {
            GridMap.SetBlock(i, 0, 0, _fenceBlock);
            GridMap.SetBlock(i, 0, _depth - 1, _fenceBlock);

            GridMap.SetBlock(i, 0, 1, _checkBlock);
            GridMap.SetBlock(i, 0, _depth - 2, _checkBlock);
        }
        for (int j = 0; j < _depth; j++)
        {
            GridMap.SetBlock(0, 0, j, _fenceBlock);
            GridMap.SetBlock(_width - 1, 0, j, _fenceBlock);
        }


        for (int j = 2; j < _depth - 1; j++)
        {
            SetRandomLine(j);
        }

        GenerateBlock(0, 0, _width, _depth);
    }

    private void SetRandomLine(int j)
    {
        GroundType type = (GroundType)Random.Range(0, (int)GroundType.EndEnum);
        for (int i = 1; i < _width - 1; i++)
        {
            GridMap.SetBlock(i, 0, j, GroundDictionary[type]);
        }
    }

    private GroundType GetRandomGT()
    {
        int randomVal = Random.Range(0, 101);

        int grade = -1;
        float cumulative = 0f;

        for (int i = 0; i < _groundList.Count; i++)
        {
            cumulative += _groundList[i].Perc;
            if (cumulative >= randomVal)
            {
                return _groundList[grade].GroundType;
            }
        }

        return GroundType.EndEnum;
    }

    public void GenerateBlock(int startX, int startZ, int endX, int endZ)
    {
        for (int j = startZ; j <= endZ; j++)
        {
            float r = Random.value;
            for (int i = startX; i < endX; i++)
            {
                Vector3 position = GridMap.GetWorldPosition(i, 0, j);
                //Debug.Log($"{i} 0 {j} {GridMap.GetBlock(i, 0, j)}");
                Ground ground = Instantiate(GridMap.GetBlock(i, 0, j), position, Quaternion.identity);

                ground.SpawnBlock(i, r, this);
            }
        }
        LoadedLineIdx = LoadedLineIdx >= endZ ? LoadedLineIdx : endZ;
    }
}
