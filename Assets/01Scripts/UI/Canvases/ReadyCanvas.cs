using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyCanvas : BasicCanvas
{
    [SerializeField] private RectTransform _lights;
    [SerializeField] private float _duration;
    [SerializeField] private float _startTime;

    private List<Image> _lightList = new List<Image>();
    private float _randomTrafTime;

    public override void OnEnterUI()
    {
        CameraManager.Instance.SetGameCam();
        AnimalManager.Instance.Player.CanIMove = true;
        currentTime = 0;
        lighter = 0;
        _randomTrafTime = Random.Range(0.5f, 2f);
        foreach (Transform item in _lights.transform)
        {
            _lightList.Add(item.GetComponent<Image>());
        }
    }

    public override void OnExitUI()
    {

    }

    private float currentTime = 0;
    private int lighter = 0;

    public override void OnUpdateUI()
    {
        currentTime += Time.deltaTime;
        if (currentTime > _startTime / 5 && lighter < 5)
        {
            currentTime = 0;
            _lightList[lighter].color = Color.red;
            _lightList[lighter + 5].color = Color.red;
            lighter++;
        }

        if (currentTime > (_startTime / 5) + _randomTrafTime)
        {
            foreach (Image item in _lightList)
            {
                item.color = Color.green;
            }
            AnimalManager.Instance.IsGaming = true;
        }

        if (AnimalManager.Instance.Player.IsMoving && !AnimalManager.Instance.IsGaming)
        {
            CameraManager.Instance.SetModelCam();
            UIManager.Instance.ChangeUIState(UIState.OnDisqualification);
        }
        else if (AnimalManager.Instance.Player.IsMoving && AnimalManager.Instance.IsGaming)
        {
            _lights.DOAnchorPosY(_lights.sizeDelta.y, _duration);
            UIManager.Instance.ChangeUIState(UIState.OnGame, _duration);
        }

        if (AnimalManager.Instance.IsGaming)
        {
            foreach (var item in AnimalManager.Instance.Users)
            {
                if (item.TryGetComponent(out AnimalAgent agent))
                {
                    agent.CanIMove = true;
                    //Debug.Log("agentcanimove");
                }
            }
        }
    }
}
