using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

[Serializable]
public class TweenShakePosition : BaseTween
{
    public Transform m_Transform;

    public float m_Strength = 0;
    public int m_Vibrota = 0;
    public float m_Randomness = 0;
    public bool m_FadeOut = true;
    protected override void CreateTween()
    {
        base.CreateTween();
        if (m_Transform == null) return;

        m_Tweener = m_Transform.DOShakePosition(m_Duration, m_Strength, m_Vibrota, m_Randomness, m_FadeOut);
    }
}
