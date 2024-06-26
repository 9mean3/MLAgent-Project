using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bench : Item
{
    [SerializeField] private float _sitTime;
    private bool _isSit = false;
    private Player _player;
    private PlayerInput _playerInput;

    private AnimalAgent _agent;
    public override void EquipItem(Transform user)
    {

        base.EquipItem(user);
        if (_isSit) return;
        _isSit = true;
        GetComponent<CapsuleCollider>().enabled = false;
        Debug.Log(user);
        if (user.TryGetComponent(out _player))
        {
            _player = user.GetComponent<Player>();
            _playerInput = user.GetComponent<PlayerInput>();

            _playerInput.Move -= _player.MoveHandler;

            _player.transform.localEulerAngles = new Vector3(0, 180, 0);
            _player.transform.position = transform.position;
        }
        else if(user.TryGetComponent(out _agent))
        {
            _agent.CanIMove = false;
            _agent.transform.localEulerAngles = new Vector3(0, 180, 0);
            _agent.transform.position = transform.position;
        }
    }

    public override void UnequipItem(Transform user)
    {
        Debug.Log(user);
        if (_player)
        {
            _playerInput.Move += _player.MoveHandler;
            _player.SetGridPosition();
            _player.ReturnMoveFar();
        }
        else if (_agent)
        {
            _agent.CanIMove = true;
            _agent.SetGridPosition();
            _agent.ReturnMoveFar();
        }

        Destroy(gameObject);
    }

    private float curTime;
    private void Update()
    {
        if (!_isSit) return;
        curTime += Time.deltaTime;
        if (curTime > _sitTime)
        {
            if (_player)
                _player.ChangeItem(null);
            else if(_agent)
                _agent.ChangeItem(null);
        }
    }
}
