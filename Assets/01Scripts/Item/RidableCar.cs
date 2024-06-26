using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidableCar : Item
{
    [SerializeField] private float _rideSpeed;
    [SerializeField] private float _rideRotSpeed;
    [SerializeField] private float _rideTime;
    private bool _isRiding;
    private Player _player;
    private PlayerInput _playerInput;

    public override void EquipItem(Transform user)
    {
        //Debug.Log("equip");
        _player = user.GetComponent<Player>();
        _playerInput = user.GetComponent<PlayerInput>();
        _playerInput.DeltaMove += MoveHandle;
        _playerInput.Move -= _player.MoveHandler;

        _player.transform.localEulerAngles = new Vector3(0, 0, 0);
        _player.transform.position = transform.position;
        transform.SetParent(user);
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        _isRiding = true;
    }

    public override void UnequipItem(Transform user)
    {
        _playerInput.DeltaMove -= MoveHandle;
        _playerInput.Move += _player.MoveHandler;

        _player.SetGridPosition();
        _player.ReturnMoveFar();

        Debug.Log("ÀýÂ÷");

        Destroy(gameObject);
    }

    private float curTime;
    private void MoveHandle(Vector2 dir)
    {
        _player.transform.Rotate(Vector3.up * dir.x * _rideRotSpeed * Time.deltaTime);
    }

    private void Update()
    {
        if (!_isRiding) return;
        _player.transform.position += _player.transform.forward * _rideSpeed * Time.deltaTime;
        //_player.transform.Translate(_player.transform.forward * _rideSpeed * Time.deltaTime);
        curTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) || curTime > _rideTime)
        {
            _player.ChangeItem(null);
        }
        Debug.Log("lesgo");
    }
}
