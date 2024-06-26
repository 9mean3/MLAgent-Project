using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Umbrella : Item
{
    [SerializeField] private float _moveAmount;
    [SerializeField] private float _moveDuration;
    [SerializeField] private float _jumpHeight;
    public override void EquipItem(Transform user)
    {
        base.EquipItem(user);
        Vector2 dir = new Vector2(0, _moveAmount);

        transform.SetParent(user);
        transform.localPosition = new Vector3(0, 0.3f, 0);

        if (user.TryGetComponent(out Player player))
        {
            player.MoveFar(dir, _moveDuration, _jumpHeight);
            player.ModJumpHeight = 1;
            player.ModMoveDuration = 0.5f;
            player.ModBasicMoveAmount = 2;
        }
        else if (user.TryGetComponent(out AnimalAgent agent))
        {
            agent.MoveFar(dir, _moveDuration, _jumpHeight);
            agent.ModJumpHeight = 1;
            agent.ModMoveDuration = 0.5f;
            agent.ModBasicMoveAmount = 2;
        }
    }

    public override void UnequipItem(Transform user)
    {
        if (user.TryGetComponent(out Player player))
        {
            player.ModJumpHeight = 0;
            player.ModMoveDuration = 0;
            player.ModBasicMoveAmount = 1;
        }
        else if (user.TryGetComponent(out AnimalAgent agent))
        {
            agent.ModJumpHeight = 0;
            agent.ModMoveDuration = 0;
            agent.ModBasicMoveAmount = 1;
        }

        Destroy(gameObject);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * 30 * Time.deltaTime);
    }
}
