using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum UIState
{
    OnModelView,
    OnReady,
    OnGame,

    OnDisqualification,
    OnDied,

    OnResult,
}

public class UIController : MonoBehaviour
{
    [SerializeField] private List<BasicCanvas> _canvasList;
    [SerializeField] private Transform _canvasParent;
    [SerializeField] private Image _fadeImage;
    public Dictionary<UIState, BasicCanvas> CanvasDictionary = new Dictionary<UIState, BasicCanvas>();
    public BasicCanvas CurrentCanvas;

    private void Start()
    {
        foreach (var item in _canvasList)
        {
            string enumName = $"On{item.name.Replace("Canvas", "")}";
            UIState UIenum = (UIState)Enum.Parse(typeof(UIState), enumName);
            CanvasDictionary.Add(UIenum, item);
        }

        CurrentCanvas = Instantiate(CanvasDictionary[UIState.OnModelView], _canvasParent);
        CurrentCanvas.OnEnterUI();

        _fadeImage.DOFade(0, 1f);
    }

    private void Update()
    {
        CurrentCanvas?.OnUpdateUI();
    }

    public void ChangeUIState(UIState nextState, float waitTime = 0)
    {
        CurrentCanvas.OnExitUI();
        Destroy(CurrentCanvas.gameObject, waitTime);
        CurrentCanvas = Instantiate(CanvasDictionary[nextState], _canvasParent);
        CurrentCanvas.OnEnterUI();
    }

    public void RestartScene()
    {
        CurrentCanvas.OnExitUI();
        CurrentCanvas = null;
        _fadeImage.DOFade(1f, 2f).OnComplete(() =>
        {
            SceneManager.LoadScene("RaceScene");
        });
    }
}
