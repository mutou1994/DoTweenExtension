using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[Serializable]
public class TweenLocalScaleXZ : BaseTween
{
    public Transform m_Transfrom;

    public Vector2 m_From = Vector2.zero;
    public Vector2 m_To = Vector2.zero;

    Vector3 _From;
    Vector3 _To;
    protected override void CreateTween()
    {
        base.CreateTween();
        if (m_Transfrom == null) return;
        _To = m_To;
        _To.y = m_Transfrom.localScale.y;
        m_Tweener = m_Transfrom.DOScale(_To, m_Duration);
    }

    public override void ResetTween()
    {
        base.ResetTween();
        if (m_Transfrom == null) return;
        Vector3 _From = m_From;
        _From.y = m_Transfrom.localScale.y;
        m_Transfrom.localScale = _From;
    }
}
