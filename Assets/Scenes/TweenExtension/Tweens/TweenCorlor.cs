using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[Serializable]
public class TweenCorlor : BaseTween
{
    public Graphic m_Graphic;

    public Color m_From = Color.white;
    public Color m_To = Color.white;
    protected override void CreateTween()
    {
        base.CreateTween();
        if (m_Graphic == null) return;

        m_Tweener = m_Graphic.DOColor(m_To, m_Duration);
    }

    public override void ResetTween()
    {
        base.ResetTween();
        if (m_Graphic == null) return;

        m_Graphic.color = m_From;
    }
}
