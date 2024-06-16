using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Ground
{
    [SerializeField] private SpawnDataSO _data;
    private int _moveDirX;
    private bool _spawner;
    private Vector3 _logDiePos;

    public override void SpawnBlock(int xIdx, float rF, GroundController controller)
    {
        _moveDirX = rF >= 0.5f ? 1 : -1;
        if (xIdx == 1)
        {
            if (_moveDirX > 0)
            {
                _spawner = true;
            }
        }
        else if (xIdx == controller.GridMap.Width - 2)
        {
            if (_moveDirX < 0)
            {
                _spawner = true;
            }
        }

        if (_spawner)
        {
            _logDiePos = new Vector3(transform.position.x + (_moveDirX * controller.GridMap.Width - 3), transform.position.y + 0.5f, transform.position.z);
            StartCoroutine(SpawnLog());
        }
    }

    private IEnumerator SpawnLog()
    {
        while (true)
        {
            float wTime = Random.Range(_data.MinSpawnTime, _data.MaxSpawnTime);
            yield return new WaitForSeconds(wTime);

            int r = Random.Range(0, _data.SpawnList.Count);
            GameObject log = Instantiate(_data.SpawnList[r], transform.position, Quaternion.LookRotation(Vector3.right * _moveDirX));
            log.GetComponent<Log>().SetDiePosition(_logDiePos);
        }

    }
}
