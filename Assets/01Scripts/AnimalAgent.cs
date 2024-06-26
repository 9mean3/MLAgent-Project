using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.Events;

public class AnimalAgent : Agent
{
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
    private bool _isDie = false;
    private Rigidbody _rb;
    private Transform _visual;
    private Vector3 _originScale;
    private int _spawnXPos; // for mlagent
    public bool CanIMove { get; set; } = true; // 기본값을 true로 설정
    public bool IsMoving { get; private set; }
    private bool _isReturning;
    private GridPosition _gridPosition;
    public GridPosition PlayerGridPosition => _gridPosition;
    private GridPosition _befGridPosition;
    private Grid _gridMap;

    private float _timeLapse;

    public Item CurrentItem;

    public override void Initialize()
    {
        _isDie = false;
        OnPlayerDie.AddListener(() =>
        {
            if (CurrentItem)
                CurrentItem.UnequipItem(transform);
        });
        _timeLapse = MoveDuration;
        _visual = transform.Find("Visual");
        _originScale = _visual.localScale;
        _rb = GetComponent<Rigidbody>();
        _groundController = FindObjectOfType<GroundController>();
        _gridMap = new Grid(_groundController.Width, 1, _groundController.Depth, 1, Vector3.zero);
        _spawnXPos = _gridMap.WorldToGridPosition(transform.position).x; 
    }

    public override void OnEpisodeBegin()
    {
        _visual.localScale = _originScale;
        _gridPosition.x = _spawnXPos;
        _gridPosition.z = 1;
        transform.position = _gridMap.GetWorldPosition(_gridPosition.x, 0, _gridPosition.z);
        _isDie = false;
        CanIMove = true; // 에피소드 시작 시 이동 가능
    }

public override void CollectObservations(VectorSensor sensor)
{
    sensor.AddObservation(_gridPosition.z); // 에이전트의 현재 Z 축 위치
    Vector3 distanceToFinishLine = _gridMap.GetWorldPosition(0, 0, AnimalManager.Instance.CheckLineIndex) - _gridMap.GetWorldPosition(_gridPosition.x, 0, _gridPosition.z);
    sensor.AddObservation(distanceToFinishLine.z); // 결승선까지의 Z 축 거리
    sensor.AddObservation(IsMoving);
    sensor.AddObservation(_isDie);
    sensor.AddObservation(_gridPosition.x); // 에이전트의 현재 X 축 위치 (추가적인 관찰값)
    sensor.AddObservation(_gridPosition.z); // 에이전트의 현재 Z 축 위치 (추가적인 관찰값)
}   

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (_isDie || !CanIMove) return;

        var moveX = actions.DiscreteActions[1] - 1; // -1, 0, 1 for left, no-op, right
        var moveZ = actions.DiscreteActions[2] - 1; // -1, 0, 1 for down, no-op, up

        Vector2 dir = new Vector2(moveX, moveZ);
        if (dir != Vector2.zero && !IsMoving)
        {
            MoveHandler(dir);
        }

        AddReward(_gridPosition.z * 0.01f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut.Clear();
        int horizontal = (int)(Input.GetAxisRaw("Horizontal") + 1);
        int vertical = (int)(Input.GetAxisRaw("Vertical") + 1);
        discreteActionsOut[0] = 0;
        discreteActionsOut[1] = horizontal;
        discreteActionsOut[2] = vertical;
    }

    private void Update()
    {
        Vector3 playerPos = _gridMap.GetWorldPosition(_gridPosition.x, 0, _gridPosition.z);
        Vector3 checklinePos = _gridMap.GetWorldPosition(0, 0, AnimalManager.Instance.CheckLineIndex);
        if (playerPos.z >= checklinePos.z)
        {
            AnimalManager.Instance.SetWinner(transform);
            AddReward(10f);
            EndEpisode();
        }

        if (_gridPosition.x > _gridMap.Width - 3 || _gridPosition.x < 3 || _gridPosition.z > _gridMap.Depth || _gridPosition.z < 1)
        {
            AddReward(-2f);
            EndEpisode();
        }
    }

    private void MoveHandler(Vector2 dir)
    {
        if (_isReturning || !CanIMove) return;

        if (!IsMoving)
        {
            StartCoroutine(MoveCor(dir * ModBasicMoveAmount, MoveDuration, MaxJumpHeight));
        }
    }

    private IEnumerator MoveCor(Vector2 dir, float moveDuration, float jumpHeight)
    {
        IsMoving = true;
        _isReturning = false;

        Vector3 startPos = _gridMap.GetWorldPosition(_gridPosition.x, 0, _gridPosition.z);
        _befGridPosition.x = (int)startPos.x;
        _befGridPosition.z = (int)startPos.z;

        _gridPosition.x += (int)dir.x;
        _gridPosition.z += (int)dir.y;
        Vector3 nextPos = _gridMap.GetWorldPosition(_gridPosition.x, 0, _gridPosition.z);

        Quaternion startRot = _visual.rotation;
        Quaternion nextRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.y));

        float time = 0;
        float duration = moveDuration;
        while (time < duration)
        {
            float t = time / duration;
            float height = 4 * jumpHeight * t * (1 - t);

            Vector3 targetVector = Vector3.Slerp(startPos, nextPos, t);
            targetVector.y = height;
            transform.localPosition = targetVector;

            Quaternion lookRot = Quaternion.LookRotation(dir);
            _visual.localRotation = Quaternion.Slerp(startRot, nextRot, t);

            time += Time.deltaTime;
            _timeLapse = time;
            yield return null;
        }

        if (dir.y > 0) AddReward(0.1f);
        else AddReward(-0.1f);
        transform.localPosition = nextPos;
        _visual.localRotation = nextRot;
        IsMoving = false;
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
        _gridPosition.x = _befGridPosition.x;
        _gridPosition.z = _befGridPosition.z;
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
        transform.position = wp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car") && !_isDie)
        {
            _visual.localScale = new Vector3(0.8f, 0.1f, 0.8f);
            _isDie = true;
            OnPlayerDie.Invoke();
            AddReward(-10f);
            EndEpisode(); // 에피소드 종료
        }
        else if (other.CompareTag("Wood"))
        {
            AddReward(-3f);
            if (CurrentItem && CurrentItem.TryGetComponent(out RidableCar car))
            {
                ChangeItem(null);
            }
            ReturnMoveFar();
        }
        else if (other.TryGetComponent(out Item item))
        {
            AddReward(-0.5f);
            //ChangeItem(item);
        }
    }

    private void DiePlayer()
    {
        _isDie = true;
        OnPlayerDie.Invoke();
        _visual.localScale = Vector3.zero;
        SetReward(-1f);
        EndEpisode();
    }
}