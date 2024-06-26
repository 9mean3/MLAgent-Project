using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelViewCanvas : BasicCanvas
{

    public override void OnEnterUI()
    {
        CameraManager.Instance.SetModelCam();
        if (AnimalManager.Instance.Player)
            AnimalManager.Instance.Player.CanIMove = false;
        foreach (var item in AnimalManager.Instance.Users)
        {
            if(TryGetComponent(out AnimalAgent agent))
            {
                //agent.CanIMove = false; wntjr wldnjfk
            }
        }
    }

    public override void OnExitUI()
    {

    }

    public override void OnUpdateUI()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UIManager.Instance.ChangeUIState(UIState.OnReady);
        }
    }
}
