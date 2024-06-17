using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Umbrella : Item
{
    [SerializeField] private float _moveAmount;
    [SerializeField] private float _moveDuration;
    [SerializeField] private float _jumpHeight;
    public override void UseItem(Transform user)
    {
        base.UseItem(user);
        Vector2 dir = new Vector2(0, _moveAmount);
        user.GetComponent<Player>().MoveFar(dir, _moveDuration, _jumpHeight);
        if (user.GetComponentInChildren<Umbrella>() == null)
        {
            transform.SetParent(user);
            transform.localPosition = new Vector3(0, 0.3f, 0);
            GetComponent<BoxCollider>().enabled = false;
            user.GetComponent<Player>().GotUmbrella = true;
            return;
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * 30 * Time.deltaTime);
    }
}
