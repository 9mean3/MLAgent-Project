using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Ground
{
    private void Awake()
    {
        //GroundType = GroundType.Water;
    }

    public override void SpawnBlock(int xIdx, float rF, GroundController controller)
    {

    }
}
