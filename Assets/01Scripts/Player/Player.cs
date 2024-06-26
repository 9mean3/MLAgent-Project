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
    public UnityEvent OnPlayerDie;
    [Header("Values")]
    [SerializeField] private float _originMaxJumpHeight;
    [SerializeField] private float _originMoveDuration;
    public int ModBasicMoveAmount { get; set; } = 1;
    public float ModJumpHeight { get; set; } = 0;
    public float ModMoveDuration { get; set; } = 0;
    public float MaxJumpHeight => _originMaxJumpHeight + ModJumpHeight;
    public float MoveDuration => _originMoveDuration + ModMoveDuration;
    [Range(0f, 1f)]
    [SerializeField] private float _bufferedInputAllowedTime;
    private bool _isDie = false;
    private Rigidbody _rb;
    private Vector2 _bufferedInputDir;
    public bool CanIMove { get; set; } = false;
    public bool IsMoving { get; private set; }
    private bool _isReturning;
    private GridPosition _gridPosition;
    public GridPosition PlayerGridPosition => _gridPosition;
    private GridPosition _befGridPosition;
    private Grid _gridMap;

    private float _timeLapse;

    public Item CurrentItem;/*{ get; private set; }*/

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
        OnPlayerDie.AddListener(() =>
        {
            if (CurrentItem)
                CurrentItem.UnequipItem(transform);
            UIManager.Instance.ChangeUIState(UIState.OnDied);
        });
        _timeLapse = MoveDuration;
        _rb = GetComponent<Rigidbody>();

        _groundController = FindObjectOfType<GroundController>();
    }

    private void Start()
    {
        _gridMap = _groundController.GridMap;
        _gridPosition.x = _gridMap.Width / 2; _gridPosition.z = 1;
        transform.position = _gridMap.GetWorldPosition(_gridPosition.x, 0, _gridPosition.z);
    }

    private void Update()
    {
        Vector3 playerPos = _gridMap.GetWorldPosition(_gridPosition.x, 0, _gridPosition.z);
        Vector3 checklinePos = _gridMap.GetWorldPosition(0, 0, AnimalManager.Instance.CheckLineIndex);
        if (playerPos.z >= checklinePos.z)
        {
            AnimalManager.Instance.SetWinner(transform);
        }
    }

    public void MoveHandler(Vector2 dir)
    {
        if (_isReturning || !CanIMove) return;

        if (_timeLapse / MoveDuration >= _bufferedInputAllowedTime)
        {
            _bufferedInputDir = dir;
        }
        if (!IsMoving)
        {
            StartCoroutine(MoveCor(_bufferedInputDir * ModBasicMoveAmount, MoveDuration, MaxJumpHeight));
            //Debug.Log(dir);
            _bufferedInputDir = Vector2.zero;
        }
    }

    private IEnumerator MoveCor(Vector2 dir, float moveDuration, float jumpHeight)
    {
        IsMoving = true;
        _isReturning = false;

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
            float height = 4 * (/*_maxJumpHeight * */jumpHeight) * t * (1 - t);

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
        IsMoving = false;



        if (_bufferedInputDir != Vector2.zero)
        {
            StartCoroutine(MoveCor(_bufferedInputDir * ModBasicMoveAmount, MoveDuration, MaxJumpHeight));
            _bufferedInputDir = Vector2.zero;
        }
    }

    private IEnumerator ReturnPosition()
    {
        _isReturning = true;
        IsMoving = false;
        Vector3 startPos = transform.position;
        Vector3 nextPos = _gridMap.GetWorldPosition(_befGridPosition.x, 0, _befGridPosition.z);

        if (Vector3.Distance(startPos, nextPos) > 3f)
        {
            nextPos = new Vector3(startPos.x, 0, startPos.z - 1);
        }

        float time = 0;
        while (time < MoveDuration)
        {
            float t = time / MoveDuration;
            float height = 4 * MaxJumpHeight * t * (1 - t);

            Vector3 targetVector = Vector3.Slerp(startPos, nextPos, t);
            targetVector.y = height;
            transform.localPosition = targetVector;

            time += Time.deltaTime;
            _timeLapse = time;
            yield return null;
        }

        transform.position = nextPos;
        _gridPosition.x = _befGridPosition.x; _gridPosition.z = _befGridPosition.z;
        _befGridPosition.z = _gridPosition.z - 1;
        _isReturning = false;
    }

    public void MoveFar(Vector2 dir, float moveDuration, float jumpHeight)
    {
        StopAllCoroutines();
        StartCoroutine(MoveCor(dir, moveDuration, jumpHeight));
    }

    public void ReturnMoveFar()
    {
        StopAllCoroutines();
        StartCoroutine(ReturnPosition());
    }

    public void ChangeItem(Item newItem)
    {
        StopAllCoroutines();
        if (CurrentItem)
        {
            CurrentItem.UnequipItem(transform);
        }
        CurrentItem = newItem;
        if (newItem)
        {
            StopAllCoroutines();
            newItem.EquipItem(transform);
        }
    }

    public void SetGridPosition()
    {
        Vector3 wp = _gridMap.GetWorldPosition((int)transform.position.x, 0, (int)transform.position.z);
        _gridPosition.x = (int)wp.x; _gridPosition.z = (int)wp.z;
        _befGridPosition.x = _gridPosition.x; _befGridPosition.z = _gridPosition.z;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other);
        if (other.CompareTag("Car") && !_isDie)
        {
            transform.Find("Visual").localScale = new Vector3(0.8f, 0.1f, 0.8f);
            _isDie = true;
            OnPlayerDie.Invoke();
        }
        else if (other.CompareTag("Wood"))
        {
            if (CurrentItem && CurrentItem.TryGetComponent(out RidableCar car))
            {
                ChangeItem(null);
            }
            ReturnMoveFar();
        }

        else if (other.TryGetComponent(out Item item))
        {
            ChangeItem(item);
        }
    }
}
