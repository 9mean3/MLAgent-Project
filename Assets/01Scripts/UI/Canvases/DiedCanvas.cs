using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DiedCanvas : BasicCanvas
{
    [SerializeField] private RectTransform _panelTrm;
    [SerializeField] private RectTransform _textTrm;
    [SerializeField] private float _duration;

    public override void OnEnterUI()
    {
        _textTrm.DOAnchorPosY(1080, 0);

        _panelTrm.GetComponent<Image>().DOFade(1, _duration);
        _textTrm.DOAnchorPosY(0, _duration);
    }

    public override void OnExitUI()
    {
        _panelTrm.GetComponent<Image>().DOFade(0, _duration);
        _textTrm.DOAnchorPosY(1080, _duration);
    }

    public override void OnUpdateUI()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UIManager.Instance.RestartScene();
        }
    }
}
