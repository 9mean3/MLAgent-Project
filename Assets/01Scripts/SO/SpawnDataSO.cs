using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Ground/SpawnDataSO")]
public class SpawnDataSO : ScriptableObject
{
    public List<GameObject> SpawnList;
    public float MinSpawnTime;
    public float MaxSpawnTime;
}
