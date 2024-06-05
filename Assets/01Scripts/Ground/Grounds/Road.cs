using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Road : Ground
{
    [SerializeField] private SpawnDataSO _data;
    private int _moveDirX;
    private bool _spawner;

    public override void SpawnBlock(int xIdx, float rF, GroundController controller)
    {
        _moveDirX = rF >= 0.5f ? 1 : -1;
        if (xIdx == 0)
        {
            if (_moveDirX > 0)
            {
                _spawner = true;
            }
        }
        else if (xIdx == controller.GridMap.Width - 1)
        {
            if (_moveDirX < 0)
            {
                _spawner = true;
            }
        }

        if (_spawner)
        {
            StartCoroutine(SpawnCar());
        }
    }

    private IEnumerator SpawnCar()
    {
        while (true)
        {
            float wTime = Random.Range(_data.MinSpawnTime, _data.MaxSpawnTime);
            yield return new WaitForSeconds(wTime);

            int r = Random.Range(0, _data.SpawnList.Count);
            Instantiate(_data.SpawnList[r], transform.position + Vector3.up * 0.5f, Quaternion.LookRotation(Vector3.right * _moveDirX));
        }

    }
}
