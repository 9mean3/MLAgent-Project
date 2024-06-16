using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour
{
    public float Speed;
    private Vector3 _diePos = Vector3.zero;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _diePos, Speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _diePos) < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    public void SetDiePosition(Vector3 diePos)
    {
        _diePos = diePos;
    }
}
