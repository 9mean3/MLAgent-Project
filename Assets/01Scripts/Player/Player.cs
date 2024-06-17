using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public struct GridPosition
{
    public int x;
    public int z;
}

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private GroundController _groundController;
    [SerializeField] private UnityEvent _onPlayerDie;
    [Header("Values")]
    [SerializeField] private float _maxJumpHeight;
    [SerializeField] private float _moveDuration;
    [Range(0f, 1f)]
    [SerializeField] private float _bufferedInputAllowedTime;
    private Rigidbody _rb;
    private Vector2 _bufferedInputDir;
    private bool _isMoving;
    private bool _isReturning;
    private GridPosition _gridPosition;
    public GridPosition PlayerGridPosition => _gridPosition;
    private GridPosition _befGridPosition;
    private Grid _gridMap;

    private float _timeLapse;

    public bool GotUmbrella;

    private void OnEnable()
    {
        _playerInput.Move += MoveHandler;
    }

    private void OnDisable()
    {
        _playerInput.Move -= MoveHandler;
    }

    private void Awake()
    {
        _timeLapse = _moveDuration;
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _gridMap = _groundController.GridMap;
        _gridPosition.x = _gridMap.Width / 2; _gridPosition.z = 1;
        transform.position = _gridMap.GetWorldPosition(_gridPosition.x, 0, _gridPosition.z);
    }



    private void MoveHandler(Vector2 dir)
    {
        if (_isReturning) return;

        if (_timeLapse / _moveDuration >= _bufferedInputAllowedTime)
        {
            _bufferedInputDir = dir;
        }
        if (!_isMoving)
        {
            if(!GotUmbrella)
            StartCoroutine(MoveCor(_bufferedInputDir, _moveDuration, _maxJumpHeight));
            else
                StartCoroutine(MoveCor(_bufferedInputDir * 2, _moveDuration * 1.8f, _maxJumpHeight * 1.8f));

            _bufferedInputDir = Vector2.zero;
        }
    }

    private IEnumerator MoveCor(Vector2 dir, float moveDuration, float jumpHeight)
    {
        Debug.Log(dir);
        _isMoving = true;

        Vector3 startPos = _gridMap.GetWorldPosition(_gridPosition.x, 0, _gridPosition.z);
        _befGridPosition.x = (int)startPos.x; _befGridPosition.z = (int)startPos.z;

        _gridPosition.x += (int)dir.x; 
        _gridPosition.z += (int)dir.y;
        Vector3 nextPos = _gridMap.GetWorldPosition(_gridPosition.x, 0, _gridPosition.z);

        Quaternion startRot = transform.rotation;
        Quaternion nextRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.y));

        float time = 0;
        float duration = moveDuration;
        while (time < duration)
        {
            float t = time / duration;
            float height = 4 * (/*_maxJumpHeight * */jumpHeight)* t * (1 - t);

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
        _rb.velocity = Vector3.zero;

        if (_bufferedInputDir != Vector2.zero)
        {
            StartCoroutine(MoveCor(_bufferedInputDir, _moveDuration, _maxJumpHeight));
            _bufferedInputDir = Vector2.zero;
        }
    }

    private IEnumerator ReturnPosition()
    {
        _isReturning = true;
        _isMoving = false;
        Vector3 startPos = transform.position;
        Vector3 nextPos = _gridMap.GetWorldPosition(_befGridPosition.x, 0, _befGridPosition.z);

        float time = 0;
        while (time < _moveDuration)
        {
            float t = time / _moveDuration;
            float height = 4 * _maxJumpHeight * t * (1 - t);

            Vector3 targetVector = Vector3.Slerp(startPos, nextPos, t);
            targetVector.y = height;
            transform.localPosition = targetVector;

            time += Time.deltaTime;
            _timeLapse = time;
            yield return null;
        }

        transform.position = nextPos;
        _gridPosition.x = _befGridPosition.x; _gridPosition.z = _befGridPosition.z;
        _isReturning = false;
    }

    public void MoveFar(Vector2 dir, float moveDuration, float jumpHeight)
    {
        StopAllCoroutines();
        StartCoroutine(MoveCor(dir, moveDuration, jumpHeight));
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other);
        if (other.CompareTag("Car"))
        {
            transform.Find("Visual").localScale = new Vector3(0.8f, 0.1f, 0.8f);
            _onPlayerDie.Invoke();
        }
        else if (other.CompareTag("Wood"))
        {
            StopAllCoroutines();

            StartCoroutine(ReturnPosition());
        }

        else if (other.TryGetComponent(out Item item))
        {
            item.UseItem(transform);
        }
    }
}
