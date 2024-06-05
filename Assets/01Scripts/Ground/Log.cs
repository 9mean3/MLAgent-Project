using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour
{
    public Grid LogGrid { get; private set; }
    public float Speed;

    private void Awake()
    {
        LogGrid = new Grid(4, 1, 1, 1, transform.position);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }
}
