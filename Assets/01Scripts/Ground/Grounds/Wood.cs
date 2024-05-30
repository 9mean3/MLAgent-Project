using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Ground
{
    List<GameObject> woodList = new List<GameObject>();

    private void Awake()
    {
        GroundType = GroundType.Wood;
    }
}
