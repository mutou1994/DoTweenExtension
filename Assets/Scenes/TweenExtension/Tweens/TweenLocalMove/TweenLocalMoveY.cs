using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[Serializable]
public class TweenLocalMoveY : BaseTween
{
    public Transform m_Transfrom;

    public float m_From = 0;
    public float m_To = 0;
    protected override void CreateTween()
    {
        base.CreateTween();
        if (m_Transfrom == null) return;

        m_Tweener = m_Transfrom.DOLocalMoveY(m_To, m_Duration);
    }

    public override void ResetTween()
    {
        base.ResetTween();
        if (m_Transfrom == null) return;

        var _From = m_Transfrom.localPosition;
        _From.y = m_From;
        m_Transfrom.localPosition = _From;
    }
}
