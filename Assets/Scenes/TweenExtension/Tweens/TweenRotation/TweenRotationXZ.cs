using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[Serializable]
public class TweenRotationXZ : BaseTween
{
    public Transform m_Transfrom;
    public RotateMode m_RotateMode = RotateMode.Fast;
    public Vector2 m_From = Vector2.zero;
    public Vector2 m_To = Vector2.zero;

    protected override void CreateTween()
    {
        base.CreateTween();
        if (m_Transfrom == null) return;
        Vector3 _To = m_To;
        _To.y = m_Transfrom.localEulerAngles.y;
        m_Tweener = m_Transfrom.DOLocalRotate(_To, m_Duration, m_RotateMode);
    }

    public override void ResetTween()
    {
        base.ResetTween();
        if (m_Transfrom == null) return;

        Vector3 _From = m_From;
        _From.y = m_Transfrom.localEulerAngles.y;
        m_Transfrom.localEulerAngles = _From;
    }
}
