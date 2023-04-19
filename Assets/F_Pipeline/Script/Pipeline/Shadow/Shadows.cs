using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Shadows
{
    private const string buffName = "Shadows";

    private CommandBuffer m_Cmd = new CommandBuffer() { name = buffName };

    private ScriptableRenderContext m_Context;
    private CullingResults m_CcullingResults;
    private FPipelineAssetShadowSetting m_ShadowSetting;

    public void SetUp(ScriptableRenderContext context, CullingResults cullingResults,
        FPipelineAssetShadowSetting shadowSetting)
    {
        m_Context = context;
        m_CcullingResults = cullingResults;
        m_ShadowSetting = shadowSetting;
    }

    public void RenderDirectionalShadows(Light light, int lightIndex)
    {
        m_Cmd.BeginSample(buffName);
        if (light.shadows != LightShadows.None && light.shadowStrength > 0f && m_CcullingResults.GetShadowCasterBounds(lightIndex, out var b))
        {
            int size = (int)m_ShadowSetting.directional.Size;
            m_Cmd.GetTemporaryRT(ShaderTag.DIRECTIONAL_LIGHT_SHADOW_MAP, size, size, 32, FilterMode.Bilinear, RenderTextureFormat.Shadowmap);
            m_Cmd.SetRenderTarget(ShaderTag.DIRECTIONAL_LIGHT_SHADOW_MAP, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.Store);
            m_Cmd.ClearRenderTarget(true, false, Color.clear);
            ExecuteBuffer();

            ShadowDrawingSettings shadowDrawingSettings = new ShadowDrawingSettings(m_CcullingResults, lightIndex);
            // m_CcullingResults.ComputeDirectionalShadowMatricesAndCullingPrimitives(lightIndex, 0, 1, Vector3.zero, )
        }
        else
        {
            m_Cmd.GetTemporaryRT(ShaderTag.DIRECTIONAL_LIGHT_SHADOW_MAP, 1,1,32,FilterMode.Bilinear, RenderTextureFormat.Shadowmap);
        }
        m_Cmd.EndSample(buffName);
    }

    public void CleanUp()
    {
        m_Cmd.ReleaseTemporaryRT(ShaderTag.DIRECTIONAL_LIGHT_SHADOW_MAP);
        ExecuteBuffer();
    }

    void ExecuteBuffer()
    {
        m_Context.ExecuteCommandBuffer(m_Cmd);
        m_Cmd.Clear();
    }
}
