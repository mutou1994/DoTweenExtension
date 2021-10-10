using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[Serializable]
public class TweenCanvasGroup : BaseTween
{
    public CanvasGroup m_CanvasGroup;

    public float m_From = 0;
    public float m_To = 0;
    protected override void CreateTween()
    {
        base.CreateTween();
        if (m_CanvasGroup == null) return;
        m_Tweener = m_CanvasGroup.DOFade(m_To, m_Duration);
    }

    public override void ResetTween()
    {
        base.ResetTween();
        if (m_CanvasGroup == null) return;
        m_CanvasGroup.alpha = m_From;
    }
}
