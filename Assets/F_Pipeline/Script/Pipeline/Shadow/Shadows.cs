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


    void ExecuteBuffer()
    {
        m_Context.ExecuteCommandBuffer(m_Cmd);
        m_Cmd.Clear();
    }
}
