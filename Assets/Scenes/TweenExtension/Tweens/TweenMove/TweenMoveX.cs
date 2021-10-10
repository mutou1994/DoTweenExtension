using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[Serializable]
public class TweenMoveX : BaseTween
{
    public Transform m_Transfrom;

    public float m_From = 0;
    public float m_To = 0;
    protected override void CreateTween()
    {
        base.CreateTween();
        if (m_Transfrom == null) return;

        m_Tweener = m_Transfrom.DOMoveX(m_To, m_Duration);
    }

    public override void ResetTween()
    {
        base.ResetTween();
        if (m_Transfrom == null) return;

        var _From = m_Transfrom.position;
        _From.x = m_From;
        m_Transfrom.position = _From;
    }
}
