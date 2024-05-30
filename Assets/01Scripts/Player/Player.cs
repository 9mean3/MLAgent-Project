using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

struct GridPostion
{
    public int x;
    public int z;
}

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private GroundController _groundController;
    [Header("Values")]
    [SerializeField] private float _maxJumpHeight;
    [SerializeField] private float _moveDuration;
    [Range(0f, 1f)]
    [SerializeField] private float _bufferedInputAllowedTime;
    private Vector2 _bufferedInputDir;
    private bool _isMoving;
    private GridPostion _gridPosition;
    private Grid _gridMap;

    private float _timeLapse;
    
    private void Awake()
    {
        _gridMap = _groundController.GridMap;
        _gridPosition.x = _gridMap.Width / 2; _gridPosition.z = 0;
        transform.position = _gridMap.GetWorldPosition(_gridPosition.x, 0, _gridPosition.z);

        _timeLapse = _moveDuration;

        _playerInput.Move += MoveHandler;
    }

    private void MoveHandler(Vector2 dir)
    {
        Debug.Log(dir);
        if(_timeLapse / _moveDuration > _bufferedInputAllowedTime)
        {
            _bufferedInputDir = dir;
        }
        if (!_isMoving)
        {
            StartCoroutine(MoveCor(_bufferedInputDir));
            _bufferedInputDir = Vector2.zero;
        }
    }

    private IEnumerator MoveCor(Vector2 dir)
    {
        _isMoving = true;

        Vector3 startPos = _gridMap.GetWorldPosition(_gridPosition.x, 0, _gridPosition.z);
        if(Mathf.Abs(dir.y) > 0)
        {
            _gridPosition.z += (int)dir.y;
        }
        else if(Mathf.Abs(dir.x) > 0)
        {
            _gridPosition.x += (int)dir.x;
        }
        Vector3 nextPos = _gridMap.GetWorldPosition(_gridPosition.x, 0, _gridPosition.z);

        Quaternion startRot = transform.rotation;
        Quaternion nextRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.y));

        float time = 0;
        while (time < _moveDuration)
        {
            float t = time / _moveDuration; 
            float height = 4 * _maxJumpHeight * t * (1 - t); 

            Vector3 targetVector = Vector3.Slerp(startPos, nextPos, t);
            targetVector.y = height; 
            transform.localPosition = targetVector;

            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.localRotation = Quaternion.Slerp(startRot, nextRot, t);

            time += Time.deltaTime;
            _timeLapse = time;
            yield return null;
        }

        transform.localPosition = nextPos;
        transform.localRotation = nextRot;
        _isMoving = false;
        
        if(_bufferedInputDir != Vector2.zero)
        {
            StartCoroutine(MoveCor(_bufferedInputDir));
            _bufferedInputDir = Vector2.zero;
        }
    }
}
