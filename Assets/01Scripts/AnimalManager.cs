using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMapManager : MonoBehaviour
{
    private static LoadMapManager _instance;
    public static LoadMapManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<LoadMapManager>();
            }
            return _instance;
        }
    }

    private List<GridPosition> _animalPosList = new List<GridPosition>();
    private GroundController _gController;

    private void Awake()
    {
        _gController = FindObjectOfType<GroundController>();
        _animalPosList.Add(FindObjectOfType<Player>().PlayerGridPosition);
    }

    private void Update()
    {
        Debug.Log(_animalPosList[0].z);
        _animalPosList.ForEach((GridPosition gp) =>
        {
            int sLoad = gp.z + 20;
            if(sLoad > _gController.LoadedLineIdx)
            {
                _gController.GenerateBlock(0, _gController.LoadedLineIdx, _gController.GridMap.Width, sLoad);
            }
        });
    }
}
