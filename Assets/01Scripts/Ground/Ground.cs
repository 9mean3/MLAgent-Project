using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GroundType
{
    Wood,
    Road,
    ItemGround,
    EndEnum,
}

public abstract class Ground : MonoBehaviour
{
    public GroundType GroundType;

    public virtual void SpawnBlock(int xIdx, float rF, GroundController controller)
    {

    }
}
