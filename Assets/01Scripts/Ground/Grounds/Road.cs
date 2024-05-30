using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Road : Ground
{
    [SerializeField] private List<Car> _carList;
    [SerializeField] private float _minSpawnTime;
    [SerializeField] private float _maxSpawnTime;

    private int _moveDirX;
    private bool _spawner;

    private void Awake()
    {
        GroundType = GroundType.Road;
    }

    public override void SpawnBlock(int idx)
    {

    }

    public void SetCarSpawner(int moveDirX)
    {
        _moveDirX = moveDirX;
        _spawner = true;
        StartCoroutine(SpawnCar());
    }

    private IEnumerator SpawnCar()
    {
        while (_spawner)
        {
            int r = Random.Range(0, _carList.Count);
            Instantiate(_carList[r], transform.position, Quaternion.LookRotation(Vector3.right * _moveDirX));

        float wTime = Random.Range(_minSpawnTime, _maxSpawnTime);
        yield return new WaitForSeconds(wTime);
        }

    }
}
