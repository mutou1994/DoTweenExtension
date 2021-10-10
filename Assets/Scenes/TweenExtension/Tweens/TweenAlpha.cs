using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[Serializable]
public class TweenAlpha : BaseTween
{
    public Graphic m_Graphic;

    public float m_From = 0;
    public float m_To = 0;
    protected override void CreateTween()
    {
        base.CreateTween();
        if (m_Graphic == null) return;
        
        m_Tweener = m_Graphic.DOFade(m_To, m_Duration);
    }

    public override void ResetTween()
    {
        base.ResetTween();
        if (m_Graphic == null) return;

        Color col = m_Graphic.color;
        col.a = m_From;
        m_Graphic.color = col;
    }
}
