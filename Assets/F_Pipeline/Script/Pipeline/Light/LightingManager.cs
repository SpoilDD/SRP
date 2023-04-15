using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LightingManager
{
    public static void RefreshLight(CullingResults cullingResults)
    {
        var lights = cullingResults.visibleLights;
        foreach (var light in lights)
        {
            if (light.lightType == LightType.Directional)
            {
                MainLight = light;
                break;
            }
        }

        if (MainLight == null)
        {
            MainLight = new VisibleLight() { finalColor = Color.clear };
        }
    }
    
    public static DirectionalLight MainLightData { get; set; }
    
    public static VisibleLight MainLight { get; set; }
}

