using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Runtime.Serialization;


public class TweenSizeDelta : BaseTween
{
    public RectTransform m_RectTransform;

    public Vector2 m_From = Vector2.zero;
    public Vector2 m_To = Vector2.zero;

    protected override void CreateTween()
    {
        base.CreateTween();
        if (m_RectTransform == null) return;

        m_Tweener = m_RectTransform.DOSizeDelta(m_To, m_Duration);
    }

    public override void ResetTween()
    {
        base.ResetTween();
        if (m_RectTransform == null) return;

        m_RectTransform.sizeDelta = m_From;
    }
}
