using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "F Asset", menuName = "F Render Pipeline/F Asset")]
public class FPipelineAsset : RenderPipelineAsset
{
    public FPipelineAssetSetting settings;
    public FPipelineAssetShadowSetting ShadowSetting;

    protected override RenderPipeline CreatePipeline()
    {
        return new FRenderPipeline(settings, ShadowSetting);
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

[Serializable]
public class FPipelineAssetShadowSetting
{
    [Min(0f)]
    public float ShadowDistance = 50f;

    public Directional directional = new Directional { Size = TextureSize._1024 };
}

public enum TextureSize
{
    _256 = 256,_512 = 512,_1024 = 1024,_2048 = 2048,_4096 = 4096,
}

[Serializable]
public struct Directional
{
    public TextureSize Size;
}
