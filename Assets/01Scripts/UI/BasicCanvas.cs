using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicCanvas : MonoBehaviour
{
    public abstract void OnEnterUI();
    public abstract void OnUpdateUI();
    public abstract void OnExitUI();
}
