using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundLine : MonoBehaviour
{
    [SerializeField] private Transform _groundParent;

    private GroundController _controller;
    public void Initialize(GroundController controller)
    {
        _controller = controller;
    }

    public void GenerateLine(GroundType groundType)
    {
        Ground ground = _controller.GroundDictionary[groundType];
    }

/*    public void GenerateRoadLine(int z, GroundType groundType)
    {
        for (int i = 0; i < _controller.GridMap.Width; i++)
        {
            _controller.GridMap.SetBlock(i, 0, z, _controller.GroundDictionary[groundType]);
            Instantiate(_controller.GridMap.GetBlock(i, 0, z), _controller.GridMap.GetWorldPosition(i, 0, z), Quaternion.identity, _groundParent);

            if (i == 0)
            {
                ((Road)ground).SetCarSpawner(1);
            }
        }
    }*/
}
