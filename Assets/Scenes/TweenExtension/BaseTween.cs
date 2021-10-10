using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Runtime.Serialization;

[Serializable]
public class BaseTween
{
    public TweenCallback onStart;
    public TweenCallback onComPlete;
    public TweenCallback onStepComplete;
    public TweenCallback onKill;

    public string m_Id;
    public Ease m_Curve = Ease.Unset;
    public AnimationCurve m_CustomCurve;

    public float m_Duration = 0.02f;
    public float m_Delay = 0f;
    public bool m_IgnoreTimeScale = true;
    public UpdateType m_UpdateType = UpdateType.Normal;
    public LoopType m_LoopType = LoopType.Restart;
    public bool m_IsLoop = false;
    public int m_LoopTime = -1;

    public Tween m_Tweener;

    public bool IsPause
    {
        get
        {
            return m_Tweener != null && m_Tweener.IsActive() && !m_Tweener.IsComplete() && !m_Tweener.IsPlaying();
        }
    }

    public bool IsPlaying
    {
        get
        {
            return m_Tweener != null && m_Tweener.IsActive() && m_Tweener.IsPlaying();
        }
    }


    public void Pause()
    {
        m_Tweener.Pause();
    }

    public void Play()
    {
        if (IsPause)
        {
            m_Tweener.Play();
        }
        else
        {
            ResetTween();
            BegainPlay();
        }
    }

    //playForwards 不会立即kill 在调过playBackwards之后Kill
    public void PlayForwards()
    {
        BegainPlay();
        m_Tweener.SetAutoKill(false);
        m_Tweener.Pause();
        m_Tweener.PlayForward();
    }

    public void PlayBackwards()
    {
        if(m_Tweener != null)
        {
            m_Tweener.OnStepComplete(()=>
            {
                if(onComPlete != null)
                {
                    onComPlete();
                }
                Kill();
            });
            m_Tweener.PlayBackwards();
        }
    }

    public void Kill()
    {
        if (m_Tweener == null) return;

        m_Tweener.Kill();
        m_Tweener = null;

    }

    void onTweenKill()
    { 
        if(onKill != null)
        {
            onKill();
        }
        if(m_Tweener.IsActive())
        {
            m_Tweener = null;
        }
    }


    protected void BegainPlay()
    {
        CreateTween();
        if (m_Tweener == null) return;

        m_Tweener.OnStart(onStart);
        m_Tweener.OnComplete(onComPlete);
        m_Tweener.OnKill(onTweenKill);

        m_Tweener.SetUpdate(m_UpdateType);
        if(m_Curve == Ease.INTERNAL_Custom)
        {
            if(m_CustomCurve == null)
            {
                m_CustomCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
            }
            m_Tweener.SetEase(m_CustomCurve);
        }
        else
        {
            m_Tweener.SetEase(m_Curve);
        }

        m_Tweener.SetDelay(m_Delay);
        m_Tweener.timeScale = m_IgnoreTimeScale ? 1 : Time.timeScale;

        if(m_LoopTime == 1 || !m_IsLoop)
        {
            m_Tweener.SetLoops(1, LoopType.Restart);
        }
        else
        {
            m_Tweener.SetLoops(m_LoopTime, m_LoopType);
        }
        
    }

    protected virtual void CreateTween() 
    {
        Kill();
    }

    public virtual void ResetTween()
    {
        Kill();
    }
}
