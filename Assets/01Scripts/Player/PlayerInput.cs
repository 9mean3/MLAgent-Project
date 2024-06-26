using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action<Vector2> Move;
    public event Action<Vector2> DeltaMove;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move?.Invoke(Vector2.up);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move?.Invoke(Vector2.down);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move?.Invoke(Vector2.left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move?.Invoke(Vector2.right);
        }

        Vector2 d = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        DeltaMove?.Invoke(d);
    }
}
