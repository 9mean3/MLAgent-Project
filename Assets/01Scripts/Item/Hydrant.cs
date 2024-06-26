using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class Hydrant : Item
{
    [SerializeField] private int _minShotAmount;
    [SerializeField] private int _maxShotAmount;
    [SerializeField] private float _moveDuration;
    [SerializeField] private float _jumpHeight;

    public override void EquipItem(Transform user)
    {
        base.EquipItem(user);
        int rX = Random.Range(_minShotAmount, _maxShotAmount) * (Random.value >= 0.5f ? 1 : -1);
        int rY = Random.Range(_minShotAmount, _maxShotAmount) * (Random.value >= 0.5f ? 1 : -1);
        Vector2 dir = new Vector2(rX, rY);
        user.GetComponent<Player>().MoveFar(dir, _moveDuration, _jumpHeight);
    }

    public override void UnequipItem(Transform user)
    {
        Destroy(gameObject);
    }
}
