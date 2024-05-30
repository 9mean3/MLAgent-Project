using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    public Dictionary<GroundType, Ground> GroundDictionary = new Dictionary<GroundType, Ground>();

    [SerializeField] private List<Ground> _groundList = new List<Ground>();
    [SerializeField] private Transform _groundParent;
    [SerializeField] private int _width;
    //[SerializeField] private int _height;
    [SerializeField] private int _depth;

    public Grid GridMap { get; private set; }

    private void Awake()
    {
        foreach (Ground item in _groundList)
        {
            GroundDictionary.Add(item.GroundType, item);
        }


        GridMap = new Grid(_width, 1, _depth, 1);

        for (int j = 0; j < _depth; j++)
        {
            GroundType type = (GroundType)Random.Range(0, (int)GroundType.EndEnum);
            Ground ground = GroundDictionary[type];

            for (int i = 0; i < _width; i++)
            {
                GridMap.SetBlock(i, 0, j, ground);
                Instantiate(GridMap.GetBlock(i, 0, j), GridMap.GetWorldPosition(i, 0, j), Quaternion.identity, _groundParent);

                if(type==GroundType.Road && j == 0)
                {
                    //(Road)ground.SetCarSpawner();
                }
            }
        }
    }
}
