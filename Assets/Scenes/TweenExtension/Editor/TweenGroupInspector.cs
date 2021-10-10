using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

[CustomEditor(typeof(TweenGroup), true)]
public class TweenGroupInspector : Editor
{
    List<Type> m_TweenTypes = new List<Type>
    {
        typeof(TweenLocalMove),
        typeof(TweenLocalMoveX), typeof(TweenLocalMoveY), typeof(TweenLocalMoveZ),
        typeof(TweenLocalMoveXY), typeof(TweenLocalMoveXZ),

        typeof(TweenMove),
        typeof(TweenMoveX), typeof(TweenMoveY), typeof(TweenMoveZ),
        typeof(TweenMoveXY), typeof(TweenMoveXZ),
        
        typeof(TweenAnchoredPos),
        typeof(TweenAnchoredPosX), typeof(TweenAnchoredPosY),

        typeof(TweenSizeDelta),
        typeof(TweenSizeDeltaX), typeof(TweenSizeDeltaY),

        typeof(TweenLocalScale),
        typeof(TweenLocalScaleX), typeof(TweenLocalScaleY), typeof(TweenLocalScaleZ),
        typeof(TweenLocalScaleXY), typeof(TweenLocalScaleXZ),

        typeof(TweenRotation),
        typeof(TweenRotationX), typeof(TweenRotationY), typeof(TweenRotationZ),
        typeof(TweenRotationXY), typeof(TweenRotationXZ),

        typeof(TweenAlpha),
        typeof(TweenCanvasGroup),
        typeof(TweenCorlor),

        typeof(TweenShakePosition),
        typeof(TweenShakeRotation),
        typeof(TweenShakeScale),
    };

    TweenGroup m_Group;
    List<bool> m_Foldouts;

    string[] m_TweenTypeEnums;
    string[] m_TweenTypeNames;

    private void OnEnable()
    {
        m_Group = target as TweenGroup;
        m_Foldouts = new List<bool>();
        m_TweenTypeNames = new string[m_TweenTypes.Count];
        m_TweenTypeEnums = new string[m_TweenTypes.Count + 1];
        m_TweenTypeEnums[0] = "AddTween";
        string name;
        for (int i=0, Imax = m_TweenTypes.Count; i < Imax; i++)
        {
            name = m_TweenTypes[i].Name.Substring(5);
            m_TweenTypeNames[i] = name;
            m_TweenTypeEnums[i+1] = name;
        }
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();

        m_Group.m_GroupId = EditorGUILayout.TextField("GroupId", m_Group.m_GroupId);

        m_Group.m_PlayOnAwake = EditorGUILayout.Toggle("PlayOnAwake", m_Group.m_PlayOnAwake);
        m_Group.m_PlayOnStart = EditorGUILayout.Toggle("PlayOnStart", m_Group.m_PlayOnStart);
        if(m_Group.m_PlayOnAwake || m_Group.m_PlayOnStart)
        {
            m_Group.m_ResetBeforeAutoPlay = EditorGUILayout.Toggle("ResetBeforeAutoPlay", m_Group.m_ResetBeforeAutoPlay);
        }

        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Play"))
        {
            if (Application.isPlaying)
            {
                m_Group.Play();
            }
        }

        if (GUILayout.Button("Pause"))
        {
            if (Application.isPlaying)
            {
                m_Group.Pause();
            }
        }

        if (GUILayout.Button("Reset"))
        {
            if (Application.isPlaying)
            {
                m_Group.ResetTween();
            }
        }
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        if(GUILayout.Button("PlayForwards"))
        {
            if (Application.isPlaying) 
            {
                m_Group.PlayForwards();
            }
        }

        if(GUILayout.Button("PlayBackwards"))
        {
            if (Application.isPlaying)
            {
                m_Group.PlayBackwards();
            }
        }
        GUILayout.EndHorizontal();

        for(int i = 0; i < m_Group.m_Tweens.Count; i++)
        {
            if(i >= m_Foldouts.Count)
            {
                m_Foldouts.Add(false);
            }
            if (!DrawTween(i))
            {
                i--;
            }
            GUILayout.Space(4);
        }

        int index = EditorGUILayout.Popup(0, m_TweenTypeEnums);
        if(index > 0)
        {
            Type type = m_TweenTypes[index - 1];
            BaseTween tw = System.Activator.CreateInstance(type) as BaseTween;
            m_Group.m_Tweens.Add(tw);
            if(m_Group.m_Tweens.Count > m_Foldouts.Count)
            {
                m_Foldouts.Add(true);
            }
            else
            {
                m_Foldouts[m_Group.m_Tweens.Count - 1] = true;
            }
        }
        Undo.RecordObject(m_Group, "TweenGroup Change");
        if (GUI.changed)
        {
            EditorUtility.SetDirty(m_Group);
        }
    }

    bool DrawTween(int index)
    {
        GUILayout.BeginHorizontal();
        BaseTween tw = m_Group.m_Tweens[index] as BaseTween;
        Type type = tw.GetType();
        int typeIndex = m_TweenTypes.FindIndex(tp => tp == type);
        m_Foldouts[index] = EditorGUILayout.Foldout(m_Foldouts[index], m_TweenTypeNames[typeIndex]);
        //因为第0个是AddTween
        int _typeIndex = EditorGUILayout.Popup(typeIndex, m_TweenTypeNames);
        if (typeIndex != _typeIndex) 
        {
            type = m_TweenTypes[_typeIndex];
            tw = System.Activator.CreateInstance(type) as BaseTween;
            m_Group.m_Tweens[index] = tw;
        }
        if(GUILayout.Button("Remove"))
        {
            m_Group.m_Tweens.RemoveAt(index);
            m_Foldouts.RemoveAt(index);
            return false;
        }
        GUILayout.EndHorizontal();

        if(index < m_Foldouts.Count && m_Foldouts[index] && index < m_Group.m_Tweens.Count)
        {
            DrawBaseProperty(m_Group.m_Tweens[index] as BaseTween);
            DrawTweenProperty(m_Group.m_Tweens[index] as BaseTween);
        }
        return true;
    }

    void DrawBaseProperty(BaseTween tw)
    {
        //BaseTween
        tw.m_Id = EditorGUILayout.TextField("ID", tw.m_Id);
        tw.m_Curve = (DG.Tweening.Ease)EditorGUILayout.EnumPopup("Ease", tw.m_Curve);
        if (tw.m_Curve == DG.Tweening.Ease.INTERNAL_Custom)
        {
            if (tw.m_CustomCurve == null)
            {
                tw.m_CustomCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
            }
            tw.m_CustomCurve = EditorGUILayout.CurveField("Curve", tw.m_CustomCurve);
        }
        else if (tw.m_CustomCurve != null)
        {
            tw.m_CustomCurve = null;
        }
        tw.m_IsLoop = EditorGUILayout.Toggle("Loop", tw.m_IsLoop);
        if (tw.m_IsLoop)
        {
            tw.m_LoopTime = EditorGUILayout.IntField("LoopTimes", tw.m_LoopTime);
            if (tw.m_LoopTime != 1)
            {
                tw.m_LoopType = (DG.Tweening.LoopType)EditorGUILayout.EnumPopup("LoopType", tw.m_LoopType);
            }
        }
        tw.m_Duration = EditorGUILayout.FloatField("Duration", tw.m_Duration);
        tw.m_Delay = EditorGUILayout.FloatField("Delay", tw.m_Delay);
        tw.m_IgnoreTimeScale = EditorGUILayout.Toggle("IgnoreTimeScale", tw.m_IgnoreTimeScale);
        tw.m_UpdateType = (DG.Tweening.UpdateType)EditorGUILayout.EnumPopup("UpdateType", tw.m_UpdateType);
    }

    void DrawTweenProperty(BaseTween tw)
    {
        if (tw is TweenLocalMove)
        {
            var _tw = tw as TweenLocalMove;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_From = EditorGUILayout.Vector3Field("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.Vector3Field("To", _tw.m_To);
        }
        else if (tw is TweenLocalMoveX)
        {
            var _tw = tw as TweenLocalMoveX;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_From = EditorGUILayout.FloatField("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.FloatField("To", _tw.m_To);
        }
        else if (tw is TweenLocalMoveY)
        {
            var _tw = tw as TweenLocalMoveY;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_From = EditorGUILayout.FloatField("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.FloatField("To", _tw.m_To);
        }
        else if (tw is TweenLocalMoveZ)
        {
            var _tw = tw as TweenLocalMoveZ;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_From = EditorGUILayout.FloatField("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.FloatField("To", _tw.m_To);
        }
        else if (tw is TweenLocalMoveXY)
        {
            var _tw = tw as TweenLocalMoveXY;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_From = EditorGUILayout.Vector2Field("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.Vector2Field("To", _tw.m_To);
        }
        else if (tw is TweenLocalMoveXZ)
        {
            var _tw = tw as TweenLocalMoveXZ;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_From = EditorGUILayout.Vector2Field("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.Vector2Field("To", _tw.m_To);
        }
        else if (tw is TweenLocalMoveYZ)
        {
            var _tw = tw as TweenLocalMoveYZ;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_From = EditorGUILayout.Vector2Field("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.Vector2Field("To", _tw.m_To);
        }
        else if (tw is TweenMove)
        {
            var _tw = tw as TweenMove;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_From = EditorGUILayout.Vector3Field("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.Vector3Field("To", _tw.m_To);
        }
        else if (tw is TweenMoveX)
        {
            var _tw = tw as TweenMoveX;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_From = EditorGUILayout.FloatField("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.FloatField("To", _tw.m_To);
        }
        else if (tw is TweenMoveY)
        {
            var _tw = tw as TweenMoveY;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_From = EditorGUILayout.FloatField("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.FloatField("To", _tw.m_To);
        }
        else if (tw is TweenMoveZ)
        {
            var _tw = tw as TweenMoveZ;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_From = EditorGUILayout.FloatField("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.FloatField("To", _tw.m_To);
        }
        else if (tw is TweenMoveXY)
        {
            var _tw = tw as TweenMoveXY;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_From = EditorGUILayout.Vector2Field("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.Vector2Field("To", _tw.m_To);
        }
        else if (tw is TweenMoveXZ)
        {
            var _tw = tw as TweenMoveXZ;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_From = EditorGUILayout.Vector2Field("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.Vector2Field("To", _tw.m_To);
        }
        else if (tw is TweenMoveYZ)
        {
            var _tw = tw as TweenMoveYZ;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_From = EditorGUILayout.Vector2Field("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.Vector2Field("To", _tw.m_To);
        }
        else if (tw is TweenAnchoredPos)
        {
            var _tw = tw as TweenAnchoredPos;
            _tw.m_RectTransform = EditorGUILayout.ObjectField("Target", _tw.m_RectTransform, typeof(RectTransform), true) as RectTransform;
            _tw.m_From = EditorGUILayout.Vector2Field("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.Vector2Field("To", _tw.m_To);
        }
        else if (tw is TweenAnchoredPosX)
        {
            var _tw = tw as TweenAnchoredPosX;
            _tw.m_RectTransform = EditorGUILayout.ObjectField("Target", _tw.m_RectTransform, typeof(RectTransform), true) as RectTransform;
            _tw.m_From = EditorGUILayout.FloatField("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.FloatField("To", _tw.m_To);
        }
        else if (tw is TweenAnchoredPosY)
        {
            var _tw = tw as TweenAnchoredPosY;
            _tw.m_RectTransform = EditorGUILayout.ObjectField("Target", _tw.m_RectTransform, typeof(RectTransform), true) as RectTransform;
            _tw.m_From = EditorGUILayout.FloatField("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.FloatField("To", _tw.m_To);
        }
        else if (tw is TweenSizeDelta)
        {
            var _tw = tw as TweenSizeDelta;
            _tw.m_RectTransform = EditorGUILayout.ObjectField("Target", _tw.m_RectTransform, typeof(RectTransform), true) as RectTransform;
            _tw.m_From = EditorGUILayout.Vector2Field("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.Vector2Field("To", _tw.m_To);
        }
        else if (tw is TweenSizeDeltaX)
        {
            var _tw = tw as TweenSizeDeltaX;
            _tw.m_RectTransform = EditorGUILayout.ObjectField("Target", _tw.m_RectTransform, typeof(RectTransform), true) as RectTransform;
            _tw.m_From = EditorGUILayout.FloatField("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.FloatField("To", _tw.m_To);
        }
        else if (tw is TweenSizeDeltaY)
        {
            var _tw = tw as TweenSizeDeltaY;
            _tw.m_RectTransform = EditorGUILayout.ObjectField("Target", _tw.m_RectTransform, typeof(RectTransform), true) as RectTransform;
            _tw.m_From = EditorGUILayout.FloatField("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.FloatField("To", _tw.m_To);
        }
        else if (tw is TweenLocalScale)
        {
            var _tw = tw as TweenLocalScale;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_From = EditorGUILayout.Vector3Field("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.Vector3Field("To", _tw.m_To);
        }
        else if (tw is TweenLocalScaleX)
        {
            var _tw = tw as TweenLocalScaleX;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_From = EditorGUILayout.FloatField("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.FloatField("To", _tw.m_To);
        }
        else if (tw is TweenLocalScaleY)
        {
            var _tw = tw as TweenLocalScaleY;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_From = EditorGUILayout.FloatField("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.FloatField("To", _tw.m_To);
        }
        else if (tw is TweenLocalScaleZ)
        {
            var _tw = tw as TweenLocalScaleZ;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_From = EditorGUILayout.FloatField("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.FloatField("To", _tw.m_To);
        }
        else if (tw is TweenLocalScaleXY)
        {
            var _tw = tw as TweenLocalScaleXY;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_From = EditorGUILayout.Vector2Field("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.Vector2Field("To", _tw.m_To);
        }
        else if (tw is TweenLocalScaleXZ)
        {
            var _tw = tw as TweenLocalScaleXZ;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_From = EditorGUILayout.Vector2Field("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.Vector2Field("To", _tw.m_To);
        }
        else if (tw is TweenLocalScaleYZ)
        {
            var _tw = tw as TweenLocalScaleYZ;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_From = EditorGUILayout.Vector2Field("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.Vector2Field("To", _tw.m_To);
        }
        else if (tw is TweenRotation)
        {
            var _tw = tw as TweenRotation;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_RotateMode = (DG.Tweening.RotateMode)EditorGUILayout.EnumPopup("RotateMode", _tw.m_RotateMode);
            _tw.m_From = EditorGUILayout.Vector3Field("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.Vector3Field("To", _tw.m_To);
        }
        else if (tw is TweenRotationX)
        {
            var _tw = tw as TweenRotationX;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_RotateMode = (DG.Tweening.RotateMode)EditorGUILayout.EnumPopup("RotateMode", _tw.m_RotateMode);
            _tw.m_From = EditorGUILayout.FloatField("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.FloatField("To", _tw.m_To);
        }
        else if (tw is TweenRotationY)
        {
            var _tw = tw as TweenRotationY;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_RotateMode = (DG.Tweening.RotateMode)EditorGUILayout.EnumPopup("RotateMode", _tw.m_RotateMode);
            _tw.m_From = EditorGUILayout.FloatField("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.FloatField("To", _tw.m_To);
        }
        else if (tw is TweenRotationZ)
        {
            var _tw = tw as TweenRotationZ;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_RotateMode = (DG.Tweening.RotateMode)EditorGUILayout.EnumPopup("RotateMode", _tw.m_RotateMode);
            _tw.m_From = EditorGUILayout.FloatField("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.FloatField("To", _tw.m_To);
        }
        else if (tw is TweenRotationXY)
        {
            var _tw = tw as TweenRotationXY;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_RotateMode = (DG.Tweening.RotateMode)EditorGUILayout.EnumPopup("RotateMode", _tw.m_RotateMode);
            _tw.m_From = EditorGUILayout.Vector2Field("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.Vector2Field("To", _tw.m_To);
        }
        else if (tw is TweenRotationXZ)
        {
            var _tw = tw as TweenRotationXZ;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_RotateMode = (DG.Tweening.RotateMode)EditorGUILayout.EnumPopup("RotateMode", _tw.m_RotateMode);
            _tw.m_From = EditorGUILayout.Vector2Field("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.Vector2Field("To", _tw.m_To);
        }
        else if (tw is TweenRotationYZ)
        {
            var _tw = tw as TweenRotationYZ;
            _tw.m_Transfrom = EditorGUILayout.ObjectField("Target", _tw.m_Transfrom, typeof(Transform), true) as Transform;
            _tw.m_RotateMode = (DG.Tweening.RotateMode)EditorGUILayout.EnumPopup("RotateMode", _tw.m_RotateMode);
            _tw.m_From = EditorGUILayout.Vector2Field("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.Vector2Field("To", _tw.m_To);
        }
        else if (tw is TweenAlpha)
        {
            var _tw = tw as TweenAlpha;
            _tw.m_Graphic = EditorGUILayout.ObjectField("Target", _tw.m_Graphic, typeof(Graphic), true) as Graphic;
            _tw.m_From = EditorGUILayout.FloatField("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.FloatField("To", _tw.m_To);
        }
        else if (tw is TweenCanvasGroup)
        {
            var _tw = tw as TweenCanvasGroup;
            _tw.m_CanvasGroup = EditorGUILayout.ObjectField("Target", _tw.m_CanvasGroup, typeof(CanvasGroup), true) as CanvasGroup;
            _tw.m_From = EditorGUILayout.FloatField("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.FloatField("To", _tw.m_To);
        }
        else if (tw is TweenCorlor)
        {
            var _tw = tw as TweenCorlor;
            _tw.m_Graphic = EditorGUILayout.ObjectField("Target", _tw.m_Graphic, typeof(Graphic), true) as Graphic;
            _tw.m_From = EditorGUILayout.ColorField("From", _tw.m_From);
            _tw.m_To = EditorGUILayout.ColorField("To", _tw.m_To);
        }
        else if (tw is TweenShakePosition)
        {
            var _tw = tw as TweenShakePosition;
            _tw.m_Transform = EditorGUILayout.ObjectField("Target", _tw.m_Transform, typeof(Transform), true) as Transform;
            _tw.m_Strength = EditorGUILayout.FloatField("Strength", _tw.m_Strength);
            _tw.m_Vibrota = EditorGUILayout.IntField("Vibrota", _tw.m_Vibrota);
            _tw.m_Randomness = EditorGUILayout.FloatField("Randomness", _tw.m_Randomness);
            _tw.m_FadeOut = EditorGUILayout.Toggle("FadeOut", _tw.m_FadeOut);
        }
        else if (tw is TweenShakeRotation)
        {
            var _tw = tw as TweenShakeRotation;
            _tw.m_Transform = EditorGUILayout.ObjectField("Target", _tw.m_Transform, typeof(Transform), true) as Transform;
            _tw.m_Strength = EditorGUILayout.FloatField("Strength", _tw.m_Strength);
            _tw.m_Vibrota = EditorGUILayout.IntField("Vibrota", _tw.m_Vibrota);
            _tw.m_Randomness = EditorGUILayout.FloatField("Randomness", _tw.m_Randomness);
            _tw.m_FadeOut = EditorGUILayout.Toggle("FadeOut", _tw.m_FadeOut);
        }
        else if (tw is TweenShakeScale)
        {
            var _tw = tw as TweenShakeScale;
            _tw.m_Transform = EditorGUILayout.ObjectField("Target", _tw.m_Transform, typeof(Transform), true) as Transform;
            _tw.m_Strength = EditorGUILayout.FloatField("Strength", _tw.m_Strength);
            _tw.m_Vibrota = EditorGUILayout.IntField("Vibrota", _tw.m_Vibrota);
            _tw.m_Randomness = EditorGUILayout.FloatField("Randomness", _tw.m_Randomness);
            _tw.m_FadeOut = EditorGUILayout.Toggle("FadeOut", _tw.m_FadeOut);
        }
    }
}
