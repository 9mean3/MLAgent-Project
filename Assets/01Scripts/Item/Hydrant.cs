using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydrant : Item
{
    [SerializeField] private int _minShotAmount;
    [SerializeField] private int _maxShotAmount;
    [SerializeField] private float _moveDuration;
    [SerializeField] private float _jumpHeight;

    public override void UseItem(Transform user)
    {
        base.UseItem(user);
        int rX = Random.Range(_minShotAmount, _maxShotAmount) * (Random.value >= 0.5f ? 1 : -1);
        int rY = Random.Range(_minShotAmount, _maxShotAmount);
        Vector2 dir = new Vector2(rX, rY);
        user.GetComponent<Player>().MoveFar(dir, _moveDuration, _jumpHeight);
    }
}
