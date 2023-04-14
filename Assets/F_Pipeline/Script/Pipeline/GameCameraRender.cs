using System;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Rendering;

public class GameCameraRender : CameraRender
{
    private CommandBuffer m_Cmd;
    private ScriptableRenderContext m_Context;
    private Camera m_Camera;
    private CullingResults m_CullingResults;
    private bool m_DynamicBatchingSwitch;
    private bool m_InstancingSwitch;

    public GameCameraRender(Camera camera)
    {
        m_Camera = camera;
    }

    public override void SetParam(bool InstancingSwitch, bool DynamicBatchingSwitch)
    {
        m_InstancingSwitch = InstancingSwitch;
        m_DynamicBatchingSwitch = DynamicBatchingSwitch;
    }

    public override void Render(ScriptableRenderContext context)
    {
        m_Context = context;
        
        if (!Cull()) return;
        
        m_Cmd = CommandBufferPool.Get(m_Camera.name);
        context.SetupCameraProperties(m_Camera);
        m_Cmd.ClearRenderTarget((m_Camera.clearFlags & CameraClearFlags.Depth) != 0, (m_Camera.clearFlags & CameraClearFlags.Color) != 0, m_Camera.backgroundColor);
        
        m_Cmd.BeginSample(m_Camera.name);
        ExecuteBuffer();


        DrawOpaque();

        context.DrawSkybox(m_Camera);

        DrawTransparent();
        
#if UNITY_EDITOR
        DrawGizmos(context, m_Camera);
#endif

        // context.ExecuteCommandBuffer(m_Cmd);
        m_Cmd.EndSample(m_Camera.name);
        ExecuteBuffer();
        m_Cmd.Release();

    }

    void ExecuteBuffer()
    {
        m_Context.ExecuteCommandBuffer(m_Cmd);
        m_Cmd.Clear();
    }

    public bool Cull()
    {
        if (!m_Camera.TryGetCullingParameters(out var parameters))
        {
            Debug.LogWarning("Culling Failed for " + parameters);
            return false;
        }

        m_CullingResults = m_Context.Cull(ref parameters);
        return true;
    }

    public void DrawOpaque()
    {

        var sortingSettings = new SortingSettings(m_Camera) { criteria = SortingCriteria.OptimizeStateChanges };
        var ds = new DrawingSettings(ShaderTag.FORWARD, sortingSettings){enableInstancing = m_InstancingSwitch, enableDynamicBatching = m_DynamicBatchingSwitch};
        var fs = new FilteringSettings(RenderQueueRange.opaque);


        m_Context.DrawRenderers(m_CullingResults, ref ds, ref fs);
    }

    private void DrawTransparent()
    {
        var sortingSettings = new SortingSettings(m_Camera) { criteria = SortingCriteria.OptimizeStateChanges };
        var ds = new DrawingSettings(ShaderTag.FORWARD, sortingSettings);
        var fs = new FilteringSettings(RenderQueueRange.transparent);
        
        m_Context.DrawRenderers(m_CullingResults, ref ds, ref fs);
    }

    public void SetCameraParam()
    {

    }

}
