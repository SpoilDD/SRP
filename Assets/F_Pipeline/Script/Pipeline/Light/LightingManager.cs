using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LightingManager
{
    public static void RefreshLight(CullingResults cullingResults)
    {
        bool isHave = false;
        var lights = cullingResults.visibleLights;
        foreach (var light in lights)
        {
            if (light.lightType == LightType.Directional)
            {
                MainLight = light;
                isHave = true;
                break;
            }
        }

        if (!isHave)
        {
            MainLight = new VisibleLight() { finalColor = Color.clear };
        }
    }

    public static DirectionalLight MainLightData = new DirectionalLight();

    private static VisibleLight mainLight;

    public static VisibleLight MainLight { get => mainLight; 
        set
        {
            mainLight = value;
            MainLightData.direction = -mainLight.localToWorldMatrix.GetColumn(2);
            MainLightData.color = mainLight.finalColor.ColorToFloat4();

        } }
}

