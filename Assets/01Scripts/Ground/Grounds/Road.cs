using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : Ground
{
    public bool isSpawner = false;

    //List<Car>

    private void Awake()
    {
        GroundType = GroundType.Road;
    }

    private void Update()
    {
        //Instantiate(car)
    }

    public void SetCarSpawner()
    {
        isSpawner = true;
    }
}
