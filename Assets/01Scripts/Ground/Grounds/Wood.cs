using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Ground
{
    [SerializeField] private WoodDataSO _woodDataSO;
    [Range(0f, 1f)]
    [SerializeField] private float _woodSpawnPerc;

    public override void SpawnBlock(int xIdx, float rF, GroundController controller)
    {
        float rWoodSpawn = Random.value;
        if(rWoodSpawn <= _woodSpawnPerc)
        {
            int rWood = Random.Range(0, _woodDataSO.WoodList.Count);
            Instantiate(_woodDataSO.WoodList[rWood], transform.position, Quaternion.identity);
        }
    }
}
