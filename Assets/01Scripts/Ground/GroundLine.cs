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
}
