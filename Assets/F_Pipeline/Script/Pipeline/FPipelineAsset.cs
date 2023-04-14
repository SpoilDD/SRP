using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "F Asset", menuName = "F Render Pipeline/F Asset")]
public class FPipelineAsset : RenderPipelineAsset
{
    public FPipelineAssetSetting settings;

    protected override RenderPipeline CreatePipeline()
    {
        return new FRenderPipeline(settings);
    }

}

[Serializable]
public class FPipelineAssetSetting
{
    [Header("Batch Settings")]
    public bool SRPBatchingSwitch = true;
    public bool DynamicBatchingSwitch = false;
    public bool InstancingSwitch = true;
}
