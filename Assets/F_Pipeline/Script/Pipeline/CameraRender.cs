using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Rendering;

abstract public class CameraRender : IDisposable
{
    virtual public void Dispose() { }
    abstract public void Render(ScriptableRenderContext context);
    
    virtual public void SetParam(bool InstancingSwitch, bool DynamicBatchingSwitch){ }

#if UNITY_EDITOR
    public void DrawGizmos(ScriptableRenderContext context, Camera camera)
    {
        if (Handles.ShouldRenderGizmos()) {
            context.DrawGizmos(camera, GizmoSubset.PreImageEffects);
            context.DrawGizmos(camera, GizmoSubset.PostImageEffects);
        }
    }
#endif
}
