using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LightingManager
{
    private Shadows m_Shadow = new Shadows();
    private static LightingManager m_Inst;

    public static LightingManager Inst
    {
        get
        {
            if (m_Inst == null)
            {
                m_Inst = new LightingManager();
            }

            return m_Inst;
        }
    }
    
    public void RefreshLight(CullingResults cullingResults)
    {
        bool isHave = false;
        var lights = cullingResults.visibleLights;
        foreach (var light in lights)
        {
            if (light.lightType == LightType.Directional)
            {
                MainLight = light;
                isHave = true;
                m_Shadow.RenderDirectionalShadows(mainLight.light, 0);
                break;
            }
        }

        if (!isHave)
        {
            MainLight = new VisibleLight() { finalColor = Color.clear };
        }
    }

    public void Render(ScriptableRenderContext context, CullingResults cullingResults,
        FPipelineAssetShadowSetting shadowSetting)
    {
        m_Shadow.SetUp(context, cullingResults, shadowSetting);
        
        
        m_Shadow.CleanUp();
    }

    public static DirectionalLight MainLightData = new DirectionalLight();

    private static VisibleLight mainLight;

    public static VisibleLight MainLight 
    { get => mainLight; 
        set
        {
            mainLight = value;
            MainLightData.direction = -mainLight.localToWorldMatrix.GetColumn(2);
            MainLightData.color = mainLight.finalColor.ColorToFloat4();

        } }
}

