using System;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;

public unsafe class GameCameraRender : CameraRender
{
    private CommandBuffer m_Cmd;
    private ScriptableRenderContext m_Context;
    private Camera m_Camera;
    private CullingResults m_CullingResults;
    private FPipelineAssetShadowSetting m_ShadowSetting;
    private FPipelineAssetSetting m_Setting;
    private ComputeBuffer m_MainLightBuffer;
    private Shadows m_Shadow = new Shadows();

    private DirectionalLight[] directionalLight;

    public GameCameraRender(Camera camera)
    {
        m_Camera = camera;
        InitComputeBuff();
    }

    public override void SetParam(FPipelineAssetSetting setting, FPipelineAssetShadowSetting shadowSetting)
    {
        m_Setting = setting;
        m_ShadowSetting = shadowSetting;
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
        
        m_Shadow.SetUp(context, m_CullingResults, m_ShadowSetting);

        SetLightData();

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

        parameters.shadowDistance = Mathf.Min(m_ShadowSetting.ShadowDistance, m_Camera.farClipPlane);
        m_CullingResults = m_Context.Cull(ref parameters);
        return true;
    }

    public void DrawOpaque()
    {
        var sortingSettings = new SortingSettings(m_Camera) { criteria = SortingCriteria.OptimizeStateChanges };
        var ds = new DrawingSettings(ShaderTag.FORWARD, sortingSettings){enableInstancing = m_Setting.InstancingSwitch, enableDynamicBatching = m_Setting.DynamicBatchingSwitch};
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

    private void SetLightData()
    {
        LightingManager.RefreshLight(m_CullingResults);
        directionalLight[0] = LightingManager.MainLightData;
        m_MainLightBuffer.SetData(directionalLight);
        m_Cmd.SetGlobalConstantBuffer(m_MainLightBuffer, ShaderTag.MAIN_LIGHT, 0, sizeof(DirectionalLight));
        ExecuteBuffer();
    }

    public void InitComputeBuff()
    {
        directionalLight = new DirectionalLight[1];
        m_MainLightBuffer = new ComputeBuffer(1, sizeof(DirectionalLight), ComputeBufferType.Constant);
        
    }

}
