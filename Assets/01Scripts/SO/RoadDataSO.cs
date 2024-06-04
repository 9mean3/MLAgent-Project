using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Ground/RoadDataSO")]
public class RoadDataSO : ScriptableObject
{
    public List<Car> CarList;
    public float MinSpawnTime;
    public float MaxSpawnTime;
}
