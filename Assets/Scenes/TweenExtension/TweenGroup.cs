using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[AddComponentMenu("DOTween/TweenGroup")]
public class TweenGroup : MonoBehaviour
{
    public string m_GroupId;
    public bool m_ResetBeforeAutoPlay = true;
    public bool m_PlayOnAwake;
    public bool m_PlayOnStart = true;

    [SerializeReference]
    public List<BaseTween> m_Tweens = new List<BaseTween>();
    private TweenCallback _onStart;
    private TweenCallback _onComplete;

    public TweenCallback onStart
    {
        get { return _onStart; }
        set
        {
            if(m_Tweens == null || m_Tweens.Count == 0)
            {
                _onStart = null;
                return;
            }
            BaseTween tw = m_Tweens[0] as BaseTween;
            for(int i=0, Imax = m_Tweens.Count; i < Imax; i++)
            {
                if(_onStart != null && (m_Tweens[i] as BaseTween).onStart != null)
                {
                    (m_Tweens[i] as BaseTween).onStart = null;
                }
                if(tw.m_Delay > (m_Tweens[i] as BaseTween).m_Delay)
                {
                    tw = (m_Tweens[i] as BaseTween);
                }
            }
            _onStart = value;
            tw.onStart = _onStart;
        }
    }

    public TweenCallback onComplete
    {
        get { return _onComplete; }
        set
        {
            if(m_Tweens == null || m_Tweens.Count == 0)
            {
                _onComplete = null;
                return;
            }
            BaseTween tw = (m_Tweens[0] as BaseTween);
            for(int i=0, Imax = m_Tweens.Count; i < Imax; i++)
            {
                if(_onComplete != null && (m_Tweens[i] as BaseTween).onComPlete != null)
                {
                    (m_Tweens[i] as BaseTween).onComPlete = null;
                }
                if(tw.m_Duration + tw.m_Delay < (m_Tweens[i] as BaseTween).m_Duration + (m_Tweens[i] as BaseTween).m_Delay)
                {
                    tw = (m_Tweens[i] as BaseTween);
                }
            }
            _onComplete = value;
            tw.onComPlete = _onComplete;
        }
    }

    private void Awake()
    {
        if (m_PlayOnAwake)
        {
            if (m_ResetBeforeAutoPlay)
            {
                ResetTween();
            }
            Play();
        }
    }

    private void Start()
    {
        if (m_PlayOnStart)
        {
            if(m_ResetBeforeAutoPlay)
            {
                ResetTween();
            }
            Play();
        }
    }

    private void OnDestroy()
    {
        onStart = null;
        onComplete = null;
        Kill();
    }

    public void Play(string id = null)
    {
        foreach(BaseTween tw in m_Tweens)
        {
            if(string.IsNullOrEmpty(id) || id.Equals(tw.m_Id))
            {
                tw.Play();
            }
        }
    }

    public void PlayForwards(string id = null)
    {
        foreach(BaseTween tw in m_Tweens)
        {
            if(string.IsNullOrEmpty(id) || id.Equals(tw.m_Id))
            {
                tw.PlayForwards();
            }
        }
    }

    public void PlayBackwards(string id = null)
    {
        foreach (BaseTween tw in m_Tweens)
        {
            if (string.IsNullOrEmpty(id) || id.Equals(tw.m_Id))
            {
                tw.PlayBackwards();
            }
        }
    }

    public void Pause(string id = null)
    {
        foreach(BaseTween tw in m_Tweens)
        {
            if(string.IsNullOrEmpty(id) || id.Equals(tw.m_Id))
            {
                tw.Pause();
            }
        }
    }

    public void Kill(string id = null)
    {
        foreach (BaseTween tw in m_Tweens)
        {
            if(string.IsNullOrEmpty(id) || id.Equals(tw.m_Id))
            {
                tw.Kill();
            }
        }
    }

    public void ResetTween(string id = null)
    {
        foreach(BaseTween tw in m_Tweens)
        {
            if(string.IsNullOrEmpty(id) || id.Equals(tw.m_Id))
            {
                tw.ResetTween();
            }
        }
    }

    public BaseTween[] GetTweensById(string id)
    {
        return null;
        if (string.IsNullOrEmpty(id))
        {
            return m_Tweens.ToArray();
        }
        return m_Tweens.FindAll(tw => id.Equals(tw.m_Id)).ToArray();
    }
}
