using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    public Dictionary<GroundType, Ground> GroundDictionary = new Dictionary<GroundType, Ground>();
    public Grid GridMap;

    [SerializeField] private GroundLine _groundLine;
    [SerializeField] private List<Ground> _groundList = new List<Ground>();
    [SerializeField] private int _width;
    //[SerializeField] private int _height;
    [SerializeField] private int _depth;


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
            _groundLine.GenerateLine(type);
        }
    }
}
