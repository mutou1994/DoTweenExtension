using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[Serializable]
public class TweenSizeDeltaY : BaseTween
{
    public RectTransform m_RectTransform;

    public float m_From = 0;
    public float m_To = 0;
    protected override void CreateTween()
    {
        base.CreateTween();
        if (m_RectTransform == null) return;

        var _To = m_RectTransform.sizeDelta;
        _To.y = m_To;
        m_Tweener = m_RectTransform.DOSizeDelta(_To, m_Duration);
    }

    public override void ResetTween()
    {
        base.ResetTween();
        if (m_RectTransform == null) return;
        var _From = m_RectTransform.sizeDelta;
        _From.y = m_From;
        m_RectTransform.sizeDelta = _From;
    }
}
