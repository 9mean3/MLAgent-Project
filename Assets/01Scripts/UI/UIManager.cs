using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{ 
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }
            return _instance;
        }
    }

    private UIController _uiController;

    private void Awake()
    {
        _uiController = FindObjectOfType<UIController>();
    }

    public void ChangeUIState(UIState nextEnum, float waitTime = 0)
    {
        _uiController.ChangeUIState(nextEnum, waitTime);
    }

    public void RestartScene()
    {
        _uiController.RestartScene();
    }
}
