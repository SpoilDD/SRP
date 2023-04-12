using System;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Rendering;

public class GameCameraRender : CameraRender
{
    CommandBuffer m_Cmd;
    Camera m_Camera;
    CullingResults m_CullingResults;

    public GameCameraRender(Camera camera)
    {
        m_Camera = camera;
    }


    public override void Render(ScriptableRenderContext context)
    {
        m_Cmd = CommandBufferPool.Get(m_Camera.name);
        m_Cmd.ClearRenderTarget((m_Camera.clearFlags & CameraClearFlags.Depth) != 0, (m_Camera.clearFlags & CameraClearFlags.Color) != 0, m_Camera.backgroundColor);

        context.SetupCameraProperties(m_Camera);

        Cull(context);

        DrawOpaque(context);

        context.DrawSkybox(m_Camera);

        context.ExecuteCommandBuffer(m_Cmd);


        m_Cmd.Release();

    }

    public void Cull(ScriptableRenderContext context)
    {
        if (!m_Camera.TryGetCullingParameters(out var parameters))
        {
            Debug.LogWarning("Culling Failed for " + parameters);
            return;
        }

        m_CullingResults = context.Cull(ref parameters);
    }

    public void DrawOpaque(ScriptableRenderContext context)
    {

        var sortingSettings = new SortingSettings(m_Camera) { criteria = SortingCriteria.OptimizeStateChanges };
        var ds = new DrawingSettings(ShaderTag.FORWARD, sortingSettings);
        var fs = new FilteringSettings(RenderQueueRange.opaque);


        context.DrawRenderers(m_CullingResults, ref ds, ref fs);
    }

    public void SetCameraParam()
    {

    }

}
