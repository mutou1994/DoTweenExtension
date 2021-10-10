using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[Serializable]
public class TweenRotation : BaseTween
{
    public Transform m_Transfrom;
    public RotateMode m_RotateMode = RotateMode.Fast;
    public Vector3 m_From = Vector2.zero;
    public Vector3 m_To = Vector2.zero;
    protected override void CreateTween()
    {
        base.CreateTween();
        if (m_Transfrom == null) return;

        m_Tweener = m_Transfrom.DOLocalRotate(m_To, m_Duration, m_RotateMode);
    }

    public override void ResetTween()
    {
        base.ResetTween();
        if (m_Transfrom == null) return;

        m_Transfrom.localEulerAngles = m_From;
    }
}
