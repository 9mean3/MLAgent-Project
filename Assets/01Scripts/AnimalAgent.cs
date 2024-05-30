/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AnimalAgent : Agent
{
    [SerializeField] private Grid _ground;
    [SerializeField] private float _moveDuration;
    private bool _isMoving;
    public override void Initialize()
    {

    }

    public override void OnEpisodeBegin()
    {

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

    }

    private void Start()
    {
        _ground.WorldToCell(transform.position);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            MoveForward();
    }

    private void MoveForward()
    {
        if (!_isMoving)
            StartCoroutine(MoveForwardCor());
    }

    private IEnumerator MoveForwardCor()
    {
        Debug.Log("movving");
        _isMoving = true;

        float time = 0;
        Vector3Int curPos = Vector3Int.FloorToInt(transform.localPosition);
        Vector3Int nextPos = curPos + new Vector3Int(0, 0, 1);
        Debug.Log($"{curPos} -> {nextPos}");
        _ground.WorldToCell(transform.position);
        while (time < _moveDuration)
        {
            Vector3 targetVector = Vector3.Lerp(curPos, nextPos, time / _moveDuration);
            transform.localPosition = targetVector;
            time += Time.deltaTime;
            yield return null;
        }
        _isMoving = false;
    }
}
*/