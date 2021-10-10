using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[Serializable]
public class TweenMove : BaseTween
{
    public Transform m_Transfrom;

    public Vector3 m_From = Vector2.zero;
    public Vector3 m_To = Vector2.zero;
    protected override void CreateTween()
    {
        base.CreateTween();
        if (m_Transfrom == null) return;
        
         m_Tweener = m_Transfrom.DOMove(m_To, m_Duration);
    }

    public override void ResetTween()
    {
        base.ResetTween();
        if (m_Transfrom == null) return;

        m_Transfrom.position = m_From;
    }
}
