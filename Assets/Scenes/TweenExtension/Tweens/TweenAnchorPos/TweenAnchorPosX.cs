using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[Serializable]
public class TweenAnchoredPosX : BaseTween
{
    public RectTransform m_RectTransform;

    public float m_From = 0;
    public float m_To = 0;
    protected override void CreateTween()
    {
        base.CreateTween();
        if (m_RectTransform == null) return;
       
        m_Tweener = m_RectTransform.DOAnchorPosX(m_To, m_Duration);
    }

    public override void ResetTween()
    {
        base.ResetTween();
        if (m_RectTransform == null) return;
        var _From = m_RectTransform.anchoredPosition;
        _From.x = m_From;
        m_RectTransform.anchoredPosition = _From;
    }
}
